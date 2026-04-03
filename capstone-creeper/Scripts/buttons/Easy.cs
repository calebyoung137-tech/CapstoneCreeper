using Godot;
using System;

public partial class Easy : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += goToEasy; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void goToEasy()
	{
		GameSettings.Mode = GameMode.SinglePlayer;

		GetTree().ChangeSceneToFile("res://Scenes/Creeper.tscn");
	}
   }
