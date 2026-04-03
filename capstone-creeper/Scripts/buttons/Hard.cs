using Godot;
using System;

public partial class Hard : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += goToHard;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void goToHard()
	{
		GameSettings.Mode = GameMode.SinglePlayer;
        GameSettings.Difficulty = AIDifficulty.Hard;

        GetTree().ChangeSceneToFile("res://Scenes/Creeper.tscn");
	}
}
