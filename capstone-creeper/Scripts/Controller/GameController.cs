using Godot;
using Model;
using View; 
using System;
using static Model.GameBoard;
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

		boardView.Connect("PinClicked", new Callable(this, nameof(HandlePinClicked)));


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
					gameBoard.makeMove(selectedPin, boardPos);
					gameBoard.clearPossibleMoves(); 
					boardView.updateBoard(gameBoard);
					controllerState = ControllerState.SelectingPin;
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
}
