using Godot;
using System;
namespace Model;
using static Model.PinType;
using static Model.TileType;
using static Model.MoveType;
using System.Collections.Generic;
using System.Data;

// struct used to store moves
//      contains:   the starting coordinates of moved pin,
//                  the ending coordinates of moved pin,
//                  the type of move played,
//					the owner of the piece affected by the move,
//					the position of the piece affected by the move
//
//		Note: affectedPieceOwner and affectedPosition only used if MoveType is not BasicMove
public struct Move
{
	public Vector2I start;
	public Vector2I end;

	public MoveType type;
	public TileType affectedPieceOwner;
	public Vector2I affectedPosition;
}

public class AIController
{
	// ========================
	// Fields
	// ========================

	private PinType[,] Pins;
	private TileType[,] Tiles;
	private bool WhitePlayer;

	private int whitePinCount;
	private int blackPinCount;
	private int whiteTileCount;
	private int blackTileCount;

	// Following constants used in heuristic evaluation
	// Can be adjusted to change performance
	const int WIN_SCORE = 100000;
	const int PATH_WEIGHT = 1000;
	const int CONNECTIVITY_WEIGHT = 20;
	const int PIN_WEIGHT = 500;
	const int TILE_WEIGHT = 10;

	// ========================
	// Constructors
	// ========================

	// Creates a new AIController class and instantiates the 
	public AIController()
	{
		Pins = new PinType[7,7];
		Tiles = new TileType[6,6];
		WhitePlayer = true;

		whitePinCount = 0;
		blackPinCount = 0;
		whiteTileCount = 0;
		blackTileCount = 0;
	}

	// Makes a copy of the provided AIController instance: OUTDATED
	//public AIController(AIController controllerToCopy)
	//{
	//	Pins = controllerToCopy.Pins;
	//	Tiles = controllerToCopy.Tiles;
	//	WhitePlayer = controllerToCopy.WhitePlayer;
	//}

	// ========================
	// Public Methods
	// ========================

	// Main function to be called
	//      Parameters: state in softserve state notation
	//      Returns: a move in softserve action notation
	public string GetBestMoveString(string state)
	{
		ConvertStateToBoard(state);
		Move bestMove;
		Minimax(5, int.MinValue, int.MaxValue, WhitePlayer, out bestMove);
		return ConvertMoveToString(bestMove);
	}

	public Move GetBestMove(string state)
	{
        ConvertStateToBoard(state);
        Move bestMove;
        Minimax(5, int.MinValue, int.MaxValue, WhitePlayer, out bestMove);
        return bestMove;
    }

	public Move GetEasyMove(string state)
	{
		Random random = new Random();
		int moveDepth = random.Next(1, 4);

		GD.Print("Playing move at depth: " + moveDepth);
        ConvertStateToBoard(state);
        Move easyMove;
        Minimax(moveDepth, int.MinValue, int.MaxValue, WhitePlayer, out easyMove);
        return easyMove;
    }

	// Possible ConvertBoardToString function?
	//      Would allow for a string to be generated in softserve state notation
	//      Helpful for using AI algorithm in gameplay
	//      May need to be contained in GameBoard class

	// ========================
	// Private Methods/Helpers
	// ========================

	// Sets Pins and Tiles 2d arrays
	//      Parameters: string of state in softserve state notation
	//      Returns: void
	private void ConvertStateToBoard(string state)
	{
        whitePinCount = 0;
        blackPinCount = 0;
        whiteTileCount = 0;
        blackTileCount = 0;

        // 49 characters represent pin grid ( 7 x 7 )
        for (int i = 0; i < 49; i++)
		{
			int row = i / 7;
			int col = i % 7;
			if (state[i] == '.')
			{
				Pins[row, col] = PinType.Empty;
			}
			else if (state[i] == 'x')
			{
				Pins[row, col] = PinType.White;
				whitePinCount++;
			}
			else if (state[i] == 'o')
			{
				Pins[row, col] = PinType.Black;
				blackPinCount++;
			}
		}

		// 36 characters represent tile grid ( 6 x 6 )
		for (int i = 0; i < 36; i++)
		{
			int row = i / 6;
			int col = i % 6;
			if (state[i + 49] == '.')
			{
				Tiles[row, col] = TileType.Empty;
			}
			else if (state[i + 49] == 'x')
			{
				Tiles[row, col] = TileType.White;
				whiteTileCount++;
			}
			else if (state[i + 49] == 'o')
			{
				Tiles[row, col] = TileType.Black;
				blackTileCount++;
			}
		}

		// 1 character represents current player
		WhitePlayer = (state[85] == 'x');
    }

	// Minimax algorithm
	//      Parameters: 
	//      Returns: A move struct containing the best move found
	//			Created with help from Claude AI
	private int Minimax(int depth, int alpha, int beta, bool isMaximizing, out Move bestMove)
	{
		bestMove = new Move { 
			start = new Vector2I(-1, -1),
			end = new Vector2I(-1, -1),
			type = MoveType.BasicMove,
			affectedPieceOwner = TileType.Empty,
			affectedPosition = new Vector2I(-1, -1)
		};

        if (InWinningPosition(TileType.White)) return WIN_SCORE + depth;
		if (InWinningPosition(TileType.Black)) return -WIN_SCORE - depth;
		if (whitePinCount == 0 || blackPinCount == 0) return 0;
        if (depth == 0) return Heuristic();

		if (isMaximizing)
		{
			int maxScore = int.MinValue;
			foreach (Move move in GetLegalMoves(PinType.White))
			{
				MakeMove(move);
				int score = Minimax(depth - 1, alpha, beta, false, out _);
				UndoMove(move);

				if (score > maxScore)
				{
					maxScore = score;
					bestMove = move;
				}

				alpha = Math.Max(alpha, maxScore);
				if (beta <= alpha) break;
			}

			return maxScore;
		}
		else
		{
			int minScore = int.MaxValue;

			foreach (Move move in GetLegalMoves(PinType.Black))
			{
				MakeMove(move);
				int score = Minimax(depth - 1, alpha, beta, true, out _);
				UndoMove(move);

				if (score < minScore)
				{
					minScore = score;
					bestMove = move;
				}

				beta = Math.Min(beta, minScore);
				if (beta <= alpha) break;
			}

			return minScore;
		}

	}

    // Converts a Move into softserve move notation
    //      Parameters: Move struct
    //      Returns: A string in softserve move notation
    private string ConvertMoveToString(Move move)
    {
        string result = "";

        int startRow = move.start.X + 1;
        int startCol = move.start.Y;
        int endRow = move.end.X + 1;
        int endCol = move.end.Y;

        result += (char)('a' + startCol);
        result += startRow;
        result += (char)('a' + endCol);
        result += endRow;

        return result;
    }

    // Modifies the gameboard to reflect a move being made
    //      Parameters: Move struct
    //		Returns: none
    private void MakeMove(Move move)
	{
		Vector2I pinStart = move.start;
		Vector2I pinDestination = move.end;

		// Moves pin
		Pins[pinDestination.X, pinDestination.Y] = Pins[pinStart.X, pinStart.Y];
		Pins[pinStart.X, pinStart.Y] = PinType.Empty;

		if (move.type == MoveType.PinCapture)
		{
			// stores capturedPin owner
			PinType capturedPin = Pins[move.affectedPosition.X, move.affectedPosition.Y];

            // Removes captured pin
            Pins[move.affectedPosition.X, move.affectedPosition.Y] = PinType.Empty;

			// updates pin count
			if (capturedPin == PinType.Black) blackPinCount--;
			else whitePinCount--;
		}
		else if (move.type == MoveType.TileCapture)
		{
			// stores current player
            TileType curPlayer = TileType.Black;
            if (Pins[pinDestination.X, pinDestination.Y] == PinType.White) curPlayer = TileType.White;

			// stores affected opponent
			TileType capturedTile = Tiles[move.affectedPosition.X, move.affectedPosition.Y];

            // Changes tile to color of current player
			if (capturedTile != curPlayer)
			{
                Tiles[move.affectedPosition.X, move.affectedPosition.Y] = curPlayer;

				// updates tile counts
				if (curPlayer == TileType.White)			whiteTileCount++;
				else if (curPlayer == TileType.Black)		blackTileCount++;

				if (capturedTile == TileType.White)			whiteTileCount--;
				else if (capturedTile == TileType.Black)	blackTileCount--;
            }
		}
	}

    // Modifies the gameboard to undo a move that was previously made
    //      Parameters: Move struct
    //		Returns: none
    private void UndoMove(Move move)
    {
        Vector2I pinStart = move.start;
        Vector2I pinDestination = move.end;

        // Moves pin back to starting position
        Pins[pinStart.X, pinStart.Y] = Pins[pinDestination.X, pinDestination.Y];
        Pins[pinDestination.X, pinDestination.Y] = PinType.Empty;

        if (move.type == MoveType.PinCapture)
        {
			// checks owner of the removed pin
			PinType removedPin = PinType.Black;
			if (move.affectedPieceOwner == TileType.White) removedPin = PinType.White;

            // Puts captured pin back
            Pins[move.affectedPosition.X, move.affectedPosition.Y] = removedPin;

			// updates pin count
			if (removedPin == PinType.White) whitePinCount++;
			else blackPinCount++;
        }
        else if (move.type == MoveType.TileCapture)
        {
			// checks current tile color
			TileType capturedColor = Tiles[move.affectedPosition.X, move.affectedPosition.Y];

			TileType originalColor = move.affectedPieceOwner;

			if (capturedColor != originalColor)
			{
				// changes tile back to original color
                Tiles[move.affectedPosition.X, move.affectedPosition.Y] = originalColor;

				// updates tile counts
				if (capturedColor == TileType.White) whiteTileCount--;
				else if (capturedColor == TileType.Black) blackTileCount--;

				if (originalColor == TileType.White) whiteTileCount++;
				else if (originalColor == TileType.Black) blackTileCount++;
            }
            
        }
    }

	// Creates a list of legal moves at the current board position
	//		parameters: PinType of curPlayer
	//		returns: list of moves
	private List<Move> GetLegalMoves(PinType curPlayer)
	{
		List<Move> moves = new List<Move>();
		int pinsFound = 0;

		PinType oppPin = PinType.Black;
		TileType oppTile = TileType.Black;
		if (curPlayer == PinType.Black)
		{
			oppPin = PinType.White;
			oppTile = TileType.White;
		}
		// Legal moves from each owned pin include:
		//		1. Moving vertical or horizontal to an adjacent empty space
		//		2. Jumping an adjacent vertical or horizontal pin with an empty space on the other side
		//		3. Jumping a tile space diagonally and placing a tile there

		// Process:
		//		1. Moving through the Pin array until I find an owned pin
		//		2. Check adjacent vertical and horizontal empty spaces (add BasicMove for each)
		//		3. In the case one of these spaces is not empty,
		//			check if the space is occupied by an opponent,
		//			and whether the space on the other side is empty (if true, add PinCapture)
		//		4. Check if adjacent diagonal spaces are empty (if yes, add TileCapture)

		// Iterate through pin array
		for (int curRow = 0; curRow < 7 && pinsFound < 8; curRow++)
		{
			for (int curCol = 0; curCol < 7 && pinsFound < 8; curCol++)
			{
				// Check to see if pin is owned by curPlayer
				if (Pins[curRow, curCol] == curPlayer)
				{
					pinsFound++;

                    // check for adjacent vertical and horizontal empty spaces
                    var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
					foreach (var (dr, dc) in directions)
					{
						if (PinInBounds(curRow + dr, curCol + dc) &&
                            Pins[curRow + dr, curCol + dc] == PinType.Empty)
						{
                            // legal BasicMove found
                            moves.Add(new Move
                            {
                                start = new Vector2I(curRow, curCol),
                                end = new Vector2I(curRow + dr, curCol + dc),
                                type = MoveType.BasicMove,
                                affectedPieceOwner = TileType.Empty,
                                affectedPosition = new Vector2I(-1, -1)
                            });
                        }
						else if (PinInBounds(curRow + 2*dr, curCol + 2*dc) &&
                                 Pins[curRow + dr, curCol + dc] == oppPin &&
                                 Pins[curRow + 2*dr, curCol + 2*dc] == PinType.Empty)
						{
                            // legal PinCapture found
                            moves.Add(new Move
                            {
                                start = new Vector2I(curRow, curCol),
                                end = new Vector2I(curRow + 2*dr, curCol + 2*dc),
                                type = MoveType.PinCapture,
                                affectedPieceOwner = oppTile,
                                affectedPosition = new Vector2I(curRow + dr, curCol + dc)
                            });
                        }
                    }

                    // check for adjacent diagonal spaces
                    var diagonals = new[] { (1, 1, 0, 0), (1, -1, 0, -1), (-1, -1, -1, -1), (-1, 1, -1, 0) };
					foreach (var (dr, dc, tileR, tileC) in diagonals)
					{
                        if (PinInBounds(curRow + dr, curCol + dc) &&
                            Pins[curRow + dr, curCol + dc] == PinType.Empty &&
							TileInBounds(curRow + tileR, curCol + tileC))
                        {
                            // legal TileCapture found
                            moves.Add(new Move
                            {
                                start = new Vector2I(curRow, curCol),
                                end = new Vector2I(curRow + dr, curCol + dc),
                                type = MoveType.TileCapture,
                                affectedPieceOwner = Tiles[curRow + tileR, curCol + tileC],
                                affectedPosition = new Vector2I(curRow + tileR, curCol + tileC)
                            });
                        }
                    }

                }

			}
		}

        // Sort moves: Captures first, then basic moves
		//		Generated by Claude AI
        moves.Sort((a, b) =>
        {
            // Pin captures are most valuable
            if (a.type == MoveType.PinCapture && b.type != MoveType.PinCapture) return -1;
            if (b.type == MoveType.PinCapture && a.type != MoveType.PinCapture) return 1;

            // Tile captures next
            if (a.type == MoveType.TileCapture && b.type == MoveType.BasicMove) return -1;
            if (b.type == MoveType.TileCapture && a.type == MoveType.BasicMove) return 1;

            return 0;
        });

        return moves;
	}

	// Checks if a coordinate for a pin is in bounds
	//		parameters: int row and int col representing coordinates of pin
	//		returns: bool representing whether the pin is in bounds
	private bool PinInBounds(int row, int col)
	{
		if (row < 0 || row > 6 || col < 0 || col > 6)
		{
			return false;
		}

		if ((row == 0 && col == 0) ||
            (row == 0 && col == 6) ||
            (row == 6 && col == 6) ||
            (row == 6 && col == 0))
		{
			return false;
		}

		return true;
    }

    // Checks if a coordinate for a tile is in bounds (includes disallowing corner tiles)
    //		parameters: int row and int col representing coordinates of tile
    //		returns: bool representing whether the tile is in bounds
    private bool TileInBounds(int row, int col)
	{
        if (row < 0 || row > 5 || col < 0 || col > 5)
        {
            return false;
        }

        if ((row == 0 && col == 0) ||
            (row == 0 && col == 5) ||
            (row == 5 && col == 5) ||
            (row == 5 && col == 0))
        {
            return false;
        }

        return true;
	}

    // Checks if there is a path between a start and an end point of all the same color
    //		Parameters: Color TileType
    //		Returns: bool of whether the provided TileType has won
    //		( Created with assistance from Claude.ai )
    private bool InWinningPosition(TileType color)
	{
		Vector2I start = new Vector2I();
		Vector2I end = new Vector2I();

		if (color == TileType.White)
		{
			// checking for white tile victory
			start.X = 0;
			start.Y = 5;

			end.X = 5;
			end.Y = 0;
		}
		else 
		{
			// checking for black tile victory
            start.X = 0;
            start.Y = 0;

            end.X = 5;
            end.Y = 5;
        }

		var visited = new HashSet<(int, int)>();
		var queue = new Queue<(int, int)>();

		queue.Enqueue((start.X, start.Y));
		visited.Add((start.X, start.Y));

		while (queue.Count > 0)
		{
			var (curRow, curCol) = queue.Dequeue();

			if (curRow == end.X && curCol == end.Y)
			{
				// algorithm has reached other end
				return true;
			}

			var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
			foreach ( var (dr, dc) in directions )
			{
				int newRow = curRow + dr;
				int newCol = curCol + dc;

				if (!visited.Contains((newRow, newCol)) && 
					(newRow >= 0 && newRow < 6 && 
					newCol >= 0 && newCol < 6
					&& Tiles[newRow, newCol] == color))
				{
					visited.Add((newRow, newCol));
					queue.Enqueue((newRow, newCol));
				}
			}
		}

		return false;
	}

	// Numerically evaluates the current state of the board
	//		A positive number represents an advantage for the white player
	//		A negative number represents an advantage for the black player
	//			Used Claude AI for assistance
	private int Heuristic()
	{
		int score = 0;

		// Path progress
		//		Lower distance favored
		//		Most important factor
		int whiteDistance = ShortestPathDistance(TileType.White);
		int blackDistance = ShortestPathDistance(TileType.Black);
		score += (blackDistance - whiteDistance) * PATH_WEIGHT;

		// Connectivity
		//		(Currently removed due to skepticism on importance)
		//int whiteConnected = CountConnectedTiles(TileType.White);
		//int blackConnected = CountConnectedTiles(TileType.Black);
		//score += (whiteConnected - blackConnected) * CONNECTIVITY_WEIGHT;

		// Pin count
		//		Higher pin count favored
		//		Impact based on difference between player's pin numbers
		int pinDiff = (whitePinCount - blackPinCount);
		score += pinDiff * Math.Abs(pinDiff) * PIN_WEIGHT;

		// Territory
		//		Very low impact
		//		Want to own more tiles than opponent
		score += (whiteTileCount - blackTileCount) * TILE_WEIGHT;

		return score;
	}

	// Uses BFS to find the shortest path between already captured tiles
	//		Parameters: TileType player
	//		Returns: Number of tiles needed to complete path
	//			Made with assistance from Claude AI
	private int ShortestPathDistance(TileType player)
	{
		Vector2I start;
		Vector2I end;

		if (player == TileType.White)
		{
			start = new Vector2I(0, 5);
			end = new Vector2I(5, 0);
		}
		else
		{
            start = new Vector2I(0, 0);
            end = new Vector2I(5, 5);
        }

		var priorityQueue = new SortedSet<(int cost, int row, int col)>();
		var visited = new HashSet<(int row, int col)>();

		priorityQueue.Add((0, start.X, start.Y));

		while (priorityQueue.Count > 0)
		{
			var (curCost, curRow, curCol) = priorityQueue.Min;
			priorityQueue.Remove(priorityQueue.Min);

			if (visited.Contains((curRow, curCol))) continue;
			visited.Add((curRow, curCol));

			if (curRow == end.X && curCol == end.Y)
			{
				return curCost;
			}

            var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var (dr, dc) in directions)
			{
				int newRow = curRow + dr;
				int newCol = curCol + dc;

				if (newRow >= 0 && newRow <= 5 &&
					newCol >= 0 && newCol <= 5 &&
					!visited.Contains((newRow, newCol)))
				{
					int moveCost = (Tiles[newRow, newCol] == player) ? 0 : 1;
					priorityQueue.Add((curCost + moveCost, newRow, newCol));
				}
			}

        }

        return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
    }

    // Uses BFS to count the tiles reachable from either corner
    //		Parameters: TileType player
	//		Returns: number of player's tiles reachable from either corner
	//			Made with assistance from Claude AI
    private int CountConnectedTiles(TileType player)
	{
        Vector2I corner1;
        Vector2I corner2;

        if (player == TileType.White)
        {
            corner1 = new Vector2I(0, 5);
            corner2 = new Vector2I(5, 0);
        }
        else
        {
            corner1 = new Vector2I(0, 0);
            corner2 = new Vector2I(5, 5);
        }

		int count1 = BFSFromPosition(corner1.X, corner1.Y, player);
		int count2 = BFSFromPosition(corner2.X, corner2.Y, player);

		return count1 + count2;
    }

	// Counts the number of a players tiles that are reachable from a designated position
	//		Parameters: starting row and col, and TileType player
	//		Returns: number of player's tiles reached from the starting position
	private int BFSFromPosition(int startRow, int startCol, TileType player)
	{
		var queue = new Queue<(int, int)>();
		var visited = new HashSet<(int, int)>();

		queue.Enqueue((startRow, startCol));
        visited.Add((startRow, startCol));

		while (queue.Count > 0)
		{
			var (curRow, curCol) = queue.Dequeue();

            var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var (dr, dc) in directions)
			{
                int newRow = curRow + dr;
                int newCol = curCol + dc;

                if (!visited.Contains((newRow, newCol)) &&
                    newRow >= 0 && newRow <= 5 &&
                    newCol >= 0 && newCol <= 5 &&
                    Tiles[newRow, newCol] == player)
                {
                    visited.Add((newRow, newCol));
                    queue.Enqueue((newRow, newCol));
                }
            }

        }

		return visited.Count;
    }

}
