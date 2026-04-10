using Godot;
using System;

public partial class Easy : Button
{
	private AudioStreamPlayer _clickSound;

	public override void _Ready()
	{
		_clickSound = GetNode<AudioStreamPlayer>("Click");
		Pressed += GoToEasy; 
	}

	private void GoToEasy()
	{
		GameSettings.Mode = GameMode.SinglePlayer;
		GameSettings.Difficulty = AIDifficulty.Easy;

		_clickSound.Play();
		GetTree().ChangeSceneToFile("res://Scenes/Creeper.tscn");
	}
}
