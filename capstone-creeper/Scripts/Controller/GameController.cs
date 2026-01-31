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
	public override void _Ready()
	{
		GameBoard gameBoard = new GameBoard();
		gameBoard.InitBoard();
		BoardView boardView = GetNode<BoardView>("../BoardView");
		boardView.updateBoard(gameBoard);

		turn = new Turn();
		turn = Turn.White;
		boardView.Connect("PinClicked", new Callable(this, nameof(HandlePinClicked)));


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void HandlePinClicked(Vector2I boardPos)
	{
		GD.Print("Pin was clicked");
	}
}
