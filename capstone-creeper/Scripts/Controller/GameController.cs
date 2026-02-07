using Godot;
using Model;
using View; 
using System;
using static Model.GameBoard;
using System.Reflection.Metadata.Ecma335;
public partial class GameController : Node
{
	//private BoardView boardView;
	// Called when the node enters the scene tree for the first time.
	enum ControllerState
	{
		SelectingPin,
		MakingMove,
	}
	private enum Turn
	{
		Black,
		White
	}
	private Turn turn;
	private ControllerState controllerState;
	public GameBoard gameBoard; 
	public BoardView boardView;
	private Vector2I selectedPin; 
	public static GameController Controller{get; set;}
	public override void _Ready()
	{
		gameBoard = new GameBoard();
		gameBoard.InitBoard();
		boardView = GetNode<BoardView>("../BoardView");
		boardView.updateBoard(gameBoard);

		turn = new Turn();
		turn = Turn.White;
		controllerState = new ControllerState();
		controllerState = ControllerState.SelectingPin; 

		selectedPin = new Vector2I();
		GD.Print("in the controller" +
			"");
		boardView.Connect("PinClicked", new Callable(this, nameof(HandlePinClicked)));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    //https://forum.godotengine.org/t/difference-between-enter-tree-and-ready/9923
    public override void _EnterTree()
    {
	//instantiate controller before rpcs
		Controller = this;
    }
    public void ApplyRemoteMove(Vector2I from, Vector2I to)
    {
        gameBoard.makeMove(from, to);
        gameBoard.clearPossibleMoves();
        boardView.updateBoard(gameBoard);

		if (turn == Turn.Black) { 
			turn= Turn.White;
		}
		else
		{
            turn = Turn.Black;
        }
		selectedPin = new Vector2I(-1,-1);
    }


    private void HandlePinClicked(Vector2I boardPos)
	{
   
        if (controllerState == ControllerState.SelectingPin)
		{
			if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.White && turn == Turn.White)
			{
				selectedPin = boardPos; 
				gameBoard.HighlightPossibleMoves(boardPos);
				controllerState = ControllerState.MakingMove;
				boardView.updateBoard(gameBoard); 
			}
			else if (gameBoard.Pins[boardPos.X, boardPos.Y] == PinType.Black && turn == Turn.Black)
			{
				selectedPin = boardPos;
				gameBoard.HighlightPossibleMoves(boardPos);
				controllerState = ControllerState.MakingMove;
				boardView.updateBoard(gameBoard);
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
				if (gameBoard.Pins[boardPos.X,boardPos.Y] == PinType.PossibleMove)
				{
					//gameBoard.makeMove(selectedPin, boardPos);
					//gameBoard.clearPossibleMoves(); 
					//boardView.updateBoard(gameBoard);
					//I'm kind of abusing the apply remote move method here and it's not the best. 
					if (GameSettings.Mode == GameMode.OnlineMultiplayer)
					{
						NetworkManager.Instance.SendMove(selectedPin, boardPos);
					}
					else 
					{
						// I'm using this because this method uses the same logic as what was above
						//sorry for making your code bad
						ApplyRemoteMove(selectedPin, boardPos);
					}
                    controllerState = ControllerState.SelectingPin;
                    selectedPin = new Vector2I(-1, -1);

				}
			}
		}
	}
}
