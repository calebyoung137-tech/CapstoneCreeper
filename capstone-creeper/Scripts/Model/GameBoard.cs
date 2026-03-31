using Godot;
using Godot.Collections;
using System;
namespace Model;
using static Model.PinType;
using static View.BoardView;

public class GameBoard
{
	const int numPins = 7;
	const int numTiles = 6;

	public PinType[,] Pins;
	public TileType[,] Tiles;
	private Dictionary<string, int> gameStateHist;

	public void InitBoard()
	{
		gameStateHist = new Dictionary<string, int>();
		gameStateHist.Add(".o.o..xx.o......xo......x.......x.....o.x.....o..xx.o.o..o.....x........................x....o.", 1);
		Pins = new PinType[7, 7];
		Tiles = new TileType[6, 6];
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				if (!(i == 0 && j == 0)
					&& !(i == 0 && j == 6)
					&& !(i == 6 && j == 0)
					&& !(i == 6 && j == 6))

				{
					Pins[i, j] = Corner;
				}
				if ((i == 0 && j == 1)
					|| (i == 0 && j == 2)
					|| (i == 1 && j == 0)
					|| (i == 2 && j == 0)
					|| (i == 6 && j == 4)
					|| (i == 6 && j == 5)
					|| (i == 4 && j == 6)
					|| (i == 5 && j == 6)
					)
				{
					Pins[i, j] = Black;
				}
				else if ((i == 0 && j == 4)
					|| (i == 0 && j == 5)
					|| (i == 1 && j == 6)
					|| (i == 2 && j == 6)
					|| (i == 6 && j == 1)
					|| (i == 6 && j == 2)
					|| (i == 4 && j == 0)
					|| (i == 5 && j == 0)
					)
				{
					Pins[i, j] = White;
				}
				else
				{
					Pins[i, j] = Empty;
				}
			}
		}
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if ((i == 0 && j == 0) || (i == 5 && j == 5))
				{
					Tiles[i, j] = TileType.Black;
				}
				else if ((i == 0 && j == 5) || (i == 5 && j == 0))
				{
					Tiles[i, j] = TileType.White;
				}
				else
				{
					Tiles[i, j] = TileType.Empty;
				}
			}
		}
	}


	public bool isValidPinHole(Vector2I pin)
	{
		if (pin.X >= 0 && pin.X <= 6 && pin.Y >= 0 && pin.Y <= 6) { return true; }
		else { return false; }
	}
	public bool isValidTile(Vector2I tile)
	{
		if (tile.X >= 0 && tile.X <= 5 && tile.Y >= 0 && tile.Y <= 5) { return true; }
		else { return false; }
	}

	public void HighlightPossibleMoves(Vector2I selectedPin)
	{

		//check diagonal directions
		{
			if (isValidPinHole(new Vector2I(selectedPin.X - 1, selectedPin.Y - 1))
				&& Pins[selectedPin.X - 1, selectedPin.Y - 1] == PinType.Empty)
			{
				Pins[selectedPin.X - 1, selectedPin.Y - 1] = PinType.PossibleMove;
			}
			if (isValidPinHole(new Vector2I(selectedPin.X + 1, selectedPin.Y + 1))
				&& Pins[selectedPin.X + 1, selectedPin.Y + 1] == PinType.Empty)
			{
				Pins[selectedPin.X + 1, selectedPin.Y + 1] = PinType.PossibleMove;
			}
			if (isValidPinHole(new Vector2I(selectedPin.X - 1, selectedPin.Y + 1))
				&& Pins[selectedPin.X - 1, selectedPin.Y + 1] == PinType.Empty)
			{
				Pins[selectedPin.X - 1, selectedPin.Y + 1] = PinType.PossibleMove;
			}
			if (isValidPinHole(new Vector2I(selectedPin.X + 1, selectedPin.Y - 1))
				&& Pins[selectedPin.X + 1, selectedPin.Y - 1] == PinType.Empty)
			{
				Pins[selectedPin.X + 1, selectedPin.Y - 1] = PinType.PossibleMove;
			}
		}

		// check for verticals
		{
			if (isValidPinHole(new Vector2I(selectedPin.X, selectedPin.Y - 1)))
			{
				if (Pins[selectedPin.X, selectedPin.Y - 1] == PinType.Empty)
				{
					Pins[selectedPin.X, selectedPin.Y - 1] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X, selectedPin.Y - 1] == PinType.White
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.Black
						&& isValidPinHole(new Vector2I(selectedPin.X, selectedPin.Y - 2))
						&& Pins[selectedPin.X, selectedPin.Y - 2] == PinType.Empty)
				{
					Pins[selectedPin.X, selectedPin.Y - 2] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X, selectedPin.Y - 1] == PinType.Black
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.White
						&& isValidPinHole(new Vector2I(selectedPin.X, selectedPin.Y - 2))
						&& Pins[selectedPin.X, selectedPin.Y - 2] == PinType.Empty)
				{
					Pins[selectedPin.X, selectedPin.Y - 2] = PinType.PossibleMove;
				}
			}
			if (isValidPinHole(new Vector2I(selectedPin.X, selectedPin.Y + 1)))
			{
				if (Pins[selectedPin.X, selectedPin.Y + 1] == PinType.Empty)
				{
					Pins[selectedPin.X, selectedPin.Y + 1] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X, selectedPin.Y + 1] == PinType.White
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.Black
						&& isValidPinHole(new Vector2I(selectedPin.X, selectedPin.Y + 2))
						&& Pins[selectedPin.X, selectedPin.Y + 2] == PinType.Empty)
				{
					Pins[selectedPin.X, selectedPin.Y + 2] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X, selectedPin.Y + 1] == PinType.Black
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.White
						&& isValidPinHole(new Vector2I(selectedPin.X, selectedPin.Y + 2))
						&& Pins[selectedPin.X, selectedPin.Y + 2] == PinType.Empty)
				{
					Pins[selectedPin.X, selectedPin.Y + 2] = PinType.PossibleMove;
				}
			}
			if (isValidPinHole(new Vector2I(selectedPin.X + 1, selectedPin.Y)))
			{
				if (Pins[selectedPin.X + 1, selectedPin.Y] == PinType.Empty)
				{
					Pins[selectedPin.X + 1, selectedPin.Y] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X + 1, selectedPin.Y] == PinType.White
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.Black
						&& isValidPinHole(new Vector2I(selectedPin.X + 2, selectedPin.Y))
						&& Pins[selectedPin.X + 2, selectedPin.Y] == PinType.Empty)
				{
					Pins[selectedPin.X + 2, selectedPin.Y] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X + 1, selectedPin.Y] == PinType.Black
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.White
						&& isValidPinHole(new Vector2I(selectedPin.X + 2, selectedPin.Y))
						&& Pins[selectedPin.X + 2, selectedPin.Y] == PinType.Empty)
				{
					Pins[selectedPin.X + 2, selectedPin.Y] = PinType.PossibleMove;
				}
			}
			if (isValidPinHole(new Vector2I(selectedPin.X - 1, selectedPin.Y)))
			{
				if (Pins[selectedPin.X - 1, selectedPin.Y] == PinType.Empty)
				{
					Pins[selectedPin.X - 1, selectedPin.Y] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X - 1, selectedPin.Y] == PinType.White
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.Black
						&& isValidPinHole(new Vector2I(selectedPin.X - 2, selectedPin.Y))
						&& Pins[selectedPin.X - 2, selectedPin.Y] == PinType.Empty)
				{
					Pins[selectedPin.X - 2, selectedPin.Y] = PinType.PossibleMove;
				}
				else if (Pins[selectedPin.X - 1, selectedPin.Y] == PinType.Black
						&& Pins[selectedPin.X, selectedPin.Y] == PinType.White
						&& isValidPinHole(new Vector2I(selectedPin.X - 2, selectedPin.Y))
						&& Pins[selectedPin.X - 2, selectedPin.Y] == PinType.Empty)
				{
					Pins[selectedPin.X - 2, selectedPin.Y] = PinType.PossibleMove;
				}
			}
		}

	}

	//will need an enum of move type, to change tile color or remove jumped pin
	public void makeMove(Vector2I pinStart, Vector2I pinDestination)
	{
		//changes tile if its a diagonal move
		if (pinStart.X != pinDestination.X && pinStart.Y != pinDestination.Y)
		{
			int tileX, tileY;
			if (pinStart.X < pinDestination.X)
			{
				tileX = pinStart.X;
			}
			else
			{
				tileX = pinDestination.X;
			}
			if (pinStart.Y < pinDestination.Y)
			{
				tileY = pinStart.Y;
			}
			else
			{
				tileY = pinDestination.Y;
			}

			if (!(tileX == 0 && tileY == 0 || tileX == 5 && tileY == 0 || tileX == 0 && tileY == 5 || tileX == 5 && tileY == 5))
			{
				if (Pins[pinStart.X, pinStart.Y] == PinType.White)
				{
					Tiles[tileX, tileY] = TileType.White;
				}
				else
				{
					Tiles[tileX, tileY] = TileType.Black;
				}
			}
		}

		// checks and removes a pin for a jump
		if ((pinStart.X == pinDestination.X || pinStart.Y == pinDestination.Y)
			&& Math.Abs(pinStart.X - pinDestination.X) == 2 || Math.Abs(pinStart.Y - pinDestination.Y) == 2)
		{
			if (pinStart.X == pinDestination.X)
			{
				Pins[pinStart.X, (pinStart.Y + pinDestination.Y) / 2] = PinType.Empty;
			}
			else
			{
				Pins[(pinStart.X + pinDestination.X) / 2, pinStart.Y] = PinType.Empty;
			}
		}
		Pins[pinDestination.X, pinDestination.Y] = Pins[pinStart.X, pinStart.Y];

		Pins[pinStart.X, pinStart.Y] = PinType.Empty;



	}
	public void clearPossibleMoves()
	{
		for (int i = 0; i < Pins.GetLength(0); i++)
		{
			for (int j = 0; j < Pins.GetLength(1); j++)
			{
				if (Pins[i, j] == PinType.PossibleMove)
				{
					Pins[i, j] = PinType.Empty;
				}
			}
		}
	}

    public GameResult checkDraw()
    {
        int blackCount = 0;
        int possibleMoveCount = 0;
        for (int i = 0; i < Pins.GetLength(0); i++)
        {
            for (int j = 0; j < Pins.GetLength(1); j++)
            {
                if (Pins[i, j] == PinType.Black)
                {
                    HighlightPossibleMoves(new Vector2I(i, j));

				}
			}
		}
		foreach (var pin in Pins)
		{
			if (pin == PinType.Black) blackCount++;
			if (pin == PinType.PossibleMove) possibleMoveCount++;
		}
		if (blackCount == 0 || possibleMoveCount == 0)
		{

            return GameResult.Draw;
        }
        clearPossibleMoves();
        int whiteCount = 0;
        possibleMoveCount = 0;
        for (int i = 0; i < Pins.GetLength(0); i++)
        {
            for (int j = 0; j < Pins.GetLength(1); j++)
            {
                if (Pins[i, j] == PinType.White)
                {
                    HighlightPossibleMoves(new Vector2I(i, j));

				}
			}
		}
		foreach (var pin in Pins)
		{
			if (pin == PinType.White) whiteCount++;
			if (pin == PinType.PossibleMove) possibleMoveCount++;
		}
		clearPossibleMoves();
		if (whiteCount == 0 || possibleMoveCount == 0)
		{

            return GameResult.Draw;
        }

		string gameState = BoardToString();

		if (!gameStateHist.ContainsKey(gameState))
		{
			gameStateHist.Add(gameState, 1);
		}
		else if (gameStateHist.ContainsKey(gameState))
		{
			gameStateHist[gameState] = gameStateHist[gameState] + 1;
		}
		if (gameStateHist[gameState] == 3)
		{
			return GameResult.Draw;
		}


        return GameResult.NotOver;

	}

    public GameResult checkWin()
    {
        bool[,] searched = new bool[6, 6];
        bool blackWon = false;
        bool whiteWon = false;
        blackWon = checkWin(new Vector2I(0, 0), searched, TileType.Black);
        whiteWon = checkWin(new Vector2I(5, 0), searched, TileType.White);
        if (blackWon) { return GameResult.BlackWin; }
        else if (whiteWon) { return GameResult.WhiteWin; }
        else { return GameResult.NotOver; }
    }

	public bool checkWin(Vector2I currentPos, bool[,] searched, TileType tileToWin)
	{
		searched[currentPos.X, currentPos.Y] = true;
		if ((tileToWin == TileType.Black && currentPos == new Vector2I(5, 5))
			|| (tileToWin == TileType.White && currentPos == new Vector2I(0, 5)))
		{
			return true;
		}
		else
		{
			bool up = false;
			bool down = false;
			bool left = false;
			bool right = false;
			if (isValidTile(new Vector2I(currentPos.X + 1, currentPos.Y))
				&& !(searched[currentPos.X + 1, currentPos.Y]))
			{
				if (Tiles[currentPos.X + 1, currentPos.Y] == tileToWin)
				{
					right = checkWin(new Vector2I(currentPos.X + 1, currentPos.Y), searched, tileToWin);
				}
			}

			if (isValidTile(new Vector2I(currentPos.X - 1, currentPos.Y))
				&& !(searched[currentPos.X - 1, currentPos.Y]))
			{
				if (Tiles[currentPos.X - 1, currentPos.Y] == tileToWin)
				{
					left = checkWin(new Vector2I(currentPos.X - 1, currentPos.Y), searched, tileToWin);
				}
			}

			if (isValidTile(new Vector2I(currentPos.X, currentPos.Y + 1))
				&& !(searched[currentPos.X, currentPos.Y + 1]))
			{
				if (Tiles[currentPos.X, currentPos.Y + 1] == tileToWin)
				{
					up = checkWin(new Vector2I(currentPos.X, currentPos.Y + 1), searched, tileToWin);
				}
			}

			if (isValidTile(new Vector2I(currentPos.X, currentPos.Y - 1))
				&& !(searched[currentPos.X, currentPos.Y - 1]))
			{
				if (Tiles[currentPos.X, currentPos.Y - 1] == tileToWin)
				{
					down = checkWin(new Vector2I(currentPos.X, currentPos.Y - 1), searched, tileToWin);
				}
			}

			if (up || down || left || right)
			{
				return true;
			}
			else
			{
				return false;
			}

		}
	}

	public string BoardToString()
	{
		string gameState = "";
		foreach (var pin in Pins)
		{
			if (pin == PinType.Black) gameState += "o";
			if (pin == PinType.White) gameState += "x";
			else gameState += ".";
		}
		foreach (var tile in Tiles)
		{
			if (tile == TileType.Black) gameState += "o";
			if (tile == TileType.White) gameState += "x";
			else gameState += ".";
		}
		return gameState;
	}

	public string BoardToAIState()
	{
		string gameState = "";
		for (int i = 0; i < 49; i++)
		{
			int row = i / 7;
			int col = i % 7;
			if (Pins[col, row] == PinType.Empty || Pins[col, row] == PinType.Corner)
			{
				gameState += ".";
			}
			else if (Pins[col, row] == PinType.White)
			{
				gameState += "x";
			}
			else if (Pins[col, row] == PinType.Black)
			{
				gameState += "o";
			}
		}

		// 36 characters represent tile grid ( 6 x 6 )
		for (int i = 0; i < 36; i++)
		{
			int row = i / 6;
			int col = i % 6;
			if (Tiles[col, row] == TileType.Empty)
			{
				gameState += ".";
			}
			else if (Tiles[col, row] == TileType.White)
			{
				gameState += "x";
			}
			else if (Tiles[col, row] == TileType.Black)
			{
				gameState += "o";
			}
		}
		return gameState;
	}
	public void eraseTowers(TileType Loser)
	{
        for (int x = 0; x < Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
				if (Tiles[x, y] == Loser)
				{
					Tiles[x, y] = TileType.Empty;
				}
                // Do something with tile
            }
        }
    }
}
