using Godot;
using Model;
using System;
using static Model.GameBoard; 
public partial class GameController : Node
{
	private BoardView boardView;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameBoard gameBoard = new GameBoard();
		gameBoard.InitBoard();
		//boardView = GetNode<BoardView>("BoardView");
		//boardView.updateBoard(gameBoard); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
