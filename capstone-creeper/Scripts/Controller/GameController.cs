using Godot;
using Model;
using View;
using System;
using static Model.GameBoard;
using System.Threading.Tasks;
public partial class GameController : Node
{
	//private BoardView boardView;
	// Called when the node enters the scene tree for the first time.
	public enum ControllerState
	{
		SelectingPin,
		MakingMove,
		GameOver,
		AIMove
	}
	public enum Turn
	{
		Black,
		White
	}
	public Turn turn { get; private set; }
	public ControllerState controllerState { get; private set; }
	public  GameBoard gameBoard;
	public static BoardView boardView;
	public Vector2I selectedPin { get; private set; }
	public string NetworkGameOver = ""; 
	public static GameController Controller { get; set; }
    public static GameEndReason LastGameEndReason { get; set; } = GameEndReason.None;
	public override void _Ready()
	{
		gameBoard = new GameBoard();
		gameBoard.InitBoard();
		boardView = GetNode<BoardView>("../BoardView");
		boardView.initializeBoard(gameBoard);

		turn = new Turn();
		turn = Turn.White;
		controllerState = new ControllerState();
		controllerState = ControllerState.SelectingPin;

		selectedPin = new Vector2I();

		boardView.Connect("PinClicked", new Callable(this, nameof(HandlePinClicked)));

	}
	public bool IsGameOver()
	{
		// Clone the board for simulation
		
		//gameBoard.clearPossibleMoves();

        if (gameBoard.checkDraw() == GameResult.Draw) return true;
        if (gameBoard.checkWin() != GameResult.NotOver) return true;

		return false;
	}
	public override void _EnterTree()
	{
		//instantiate controller before rpcs
		Controller = this;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
   
	public void ApplyMove(Vector2I from, Vector2I to)
	{
		gameBoard.makeMove(from, to);
		gameBoard.clearPossibleMoves();
		boardView.updateBoard(gameBoard);

		if (turn == Turn.Black)
		{
			turn = Turn.White;
		}
		else
		{
			turn = Turn.Black;
		}
		selectedPin = new Vector2I(-1, -1);
	}

	private async void HandlePinClicked(Vector2I boardPos)
	{
		if (
			(gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.White && turn == Turn.White
			|| gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.Black && turn == Turn.Black)
			&& controllerState == ControllerState.MakingMove
			&& boardPos != selectedPin)
		{
			selectedPin = new Vector2I(-1, -1);
			controllerState = ControllerState.SelectingPin;
            gameBoard.clearPossibleMoves();
            boardView.updateBoard(gameBoard);
            HandlePinClicked(boardPos); 
		}
		else if (controllerState == ControllerState.SelectingPin)
		{
			if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.White && turn == Turn.White)
			{
				if (GameSettings.Mode == GameMode.OnlineMultiplayer)
				{
					if (Multiplayer.IsServer())
					{
						selectedPin = boardPos;
						gameBoard.HighlightPossibleMoves(boardPos);
						controllerState = ControllerState.MakingMove;
						boardView.updateBoard(gameBoard);
					}
				}
				else
				{
					selectedPin = boardPos;
					gameBoard.HighlightPossibleMoves(boardPos);
					controllerState = ControllerState.MakingMove;
					boardView.updateBoard(gameBoard);
				}
			}
			else if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.Black && turn == Turn.Black)
			{
				if (GameSettings.Mode == GameMode.OnlineMultiplayer)
				{
					if (!Multiplayer.IsServer())
					{
						selectedPin = boardPos;
						gameBoard.HighlightPossibleMoves(boardPos);
						controllerState = ControllerState.MakingMove;
						boardView.updateBoard(gameBoard);
					}
				}
				else
				{
					selectedPin = boardPos;
					gameBoard.HighlightPossibleMoves(boardPos);
					controllerState = ControllerState.MakingMove;
					boardView.updateBoard(gameBoard);
				}
			}
		}
		else if (controllerState == ControllerState.MakingMove)
		{
			if (boardPos == selectedPin)
			{
				gameBoard.clearPossibleMoves();
				controllerState = ControllerState.SelectingPin;
				selectedPin = new Vector2I(-1, -1);
				boardView.updateBoard(gameBoard);
			}
			else
			{
				if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.PossibleMove)
				{

					if (GameSettings.Mode == GameMode.OnlineMultiplayer)
					{ // if multiplayer, send the move, and apply locally
						GetNode<NetworkManager>("/root/Network").SendMove(selectedPin, boardPos);
					}

					gameBoard.makeMove(selectedPin, boardPos);
					gameBoard.clearPossibleMoves();
					boardView.updateBoard(gameBoard);
					selectedPin = new Vector2I(-1, -1);

					if (gameBoard.checkWin() != GameResult.NotOver)
					{
						controllerState = ControllerState.GameOver;
                        GameResult winner = gameBoard.checkWin();
                        if (winner == GameResult.BlackWin)
                        {
                            gameBoard.eraseTowers(TileType.White);
                        }
                        else
                        {
                            gameBoard.eraseTowers(TileType.Black);
                        }
                        boardView.updateBoard(gameBoard);
                        controllerState = ControllerState.GameOver;

                        await Task.Delay(4000);
                        boardView.gameOver(winner);

                    }
					else if (gameBoard.checkDraw() == GameResult.Draw)
					{
						controllerState = ControllerState.GameOver;
                        
                        await Task.Delay(1500);
                        boardView.gameOver(GameResult.Draw);

                    }
					else if (GameSettings.Mode == GameMode.SinglePlayer) 
					{
						// Code for playing against AI opponent
						turn = (turn == Turn.White) ? Turn.Black : Turn.White;
						controllerState = ControllerState.AIMove;

						AIController ai = new AIController();

						string state = gameBoard.BoardToAIState();
						state += (turn == Turn.White) ? "x" : "o";

						Move aiMove;
						if (GameSettings.Difficulty == AIDifficulty.Easy)
						{
                            aiMove = ai.GetEasyMove(state);
							GD.Print("Difficulty: easy");
                        }
						else
						{
							aiMove = ai.GetBestMove(state);
                            GD.Print("Difficulty: hard");
                        }

						await Task.Delay(800);

						// Highlights pin to be moved
						selectedPin = new Vector2I(aiMove.start.Y, aiMove.start.X);
						controllerState = ControllerState.MakingMove;
						boardView.updateBoard(gameBoard);
						controllerState = ControllerState.AIMove;

						await Task.Delay(800);

						// Coordinates had to be swapped due to an issue where the AI code and gameboard code work on different coordinate systems
						//		AI uses (row, col)
						//		gameBoard uses (X, Y)
						gameBoard.makeMove(new Vector2I(aiMove.start.Y, aiMove.start.X), new Vector2I(aiMove.end.Y, aiMove.end.X));
						gameBoard.clearPossibleMoves();
						boardView.updateBoard(gameBoard);

						if (gameBoard.checkWin() != GameResult.NotOver)
						{
							controllerState = ControllerState.GameOver;
                            GameResult winner = gameBoard.checkWin();
                            if (winner == GameResult.BlackWin)
                            {
                                gameBoard.eraseTowers(TileType.White);
                            }
                            else
                            {
                                gameBoard.eraseTowers(TileType.Black);
                            }
                            boardView.updateBoard(gameBoard);
                            controllerState = ControllerState.GameOver;

                            await Task.Delay(4000);
                            boardView.gameOver(GameResult.BlackWin); 
                        }
						else if (gameBoard.checkDraw() == GameResult.Draw)
						{
							controllerState = ControllerState.GameOver;
                            controllerState = ControllerState.GameOver;

                            await Task.Delay(1500);
                            boardView.gameOver(GameResult.Draw);
                        }
						else
						{
							turn = (turn == Turn.White) ? Turn.Black : Turn.White;
							selectedPin = new Vector2I(-1, -1);
							controllerState = ControllerState.SelectingPin;
						}
					}
					else
					{
						turn = (turn == Turn.White) ? Turn.Black : Turn.White;
						selectedPin = new Vector2I(-1, -1);
						controllerState = ControllerState.SelectingPin;
					}
					
				}
			}
		}
	}

	public void eraseTowers(TileType loser)
	{
		gameBoard.eraseTowers(loser); 
	}
	public void updateBoard()
	{
		boardView.updateBoard(gameBoard); 
	}
}
/*                    if (GameSettings.Mode == GameMode.OnlineMultiplayer)
                    { // if multiplayer, send the move, and apply locally
                        GetNode<NetworkManager>("/root/Network").SendMove(selectedPin, boardPos);
                    }
                    gameBoard.makeMove(selectedPin, boardPos);
                    gameBoard.clearPossibleMoves();
                    boardView.updateBoard(gameBoard);
                    if (gameBoard.checkWin() != GameResult.NotOver)
                    {
                        GameResult winner = gameBoard.checkWin();
                        if (winner == GameResult.BlackWin)
                        {
                            gameBoard.eraseTowers(TileType.White);
                        }
                        else
                        {
                            gameBoard.eraseTowers(TileType.Black);
                        }
                        boardView.updateBoard(gameBoard);
                        controllerState = ControllerState.GameOver;

                    }
                    else if (gameBoard.checkDraw() == GameResult.Draw)
                    {
                        controllerState = ControllerState.GameOver;
                        
                    }
                    else
                    {
                        controllerState = ControllerState.SelectingPin;
                    }
                    if (turn == Turn.White)
                    {
                        turn = Turn.Black;
                    }
                    else
                    {
                        turn = Turn.White;
                    }
                    selectedPin = new Vector2I(-1, -1);

                }
            }
        }
    }
}*/