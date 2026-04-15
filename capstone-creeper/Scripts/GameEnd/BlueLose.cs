using Godot;
using System;
using System.Threading.Tasks;
using View;

public partial class BlueLose : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Pressed += forceRedWin;
		if (GameSettings.Mode != GameMode.LocalMultiplayer)
		{
			TileMapLayer parent = GetParent<TileMapLayer>();
			parent.Visible = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void forceRedWin()
	{
		TextureButton Exit = GetParent().GetParent().GetParent().GetNode<TextureButton>("exit");
		Exit.Disabled = true; 
		GameController.Controller.eraseTowers(Model.TileType.Black);
		GameController.Controller.updateBoard();
		await Task.Delay(2500);
		GameController.boardView.gameOver(Model.GameResult.WhiteWin);
	}
}
