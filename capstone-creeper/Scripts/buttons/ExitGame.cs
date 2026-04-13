using Godot;
using System;

public partial class ExitGame : TextureButton
{
	public override void _Ready()
	{
		Pressed += OnExitPressed; 
	}

	public void OnExitPressed()
	{
		GetTree().Quit();
	}
}
