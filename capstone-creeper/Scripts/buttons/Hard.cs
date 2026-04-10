using Godot;
using System;

public partial class Hard : Button
{
	private AudioStreamPlayer _clickSound;

	public override void _Ready()
	{
		_clickSound = GetNode<AudioStreamPlayer>("Click");
		Pressed += GoToHard;
	}

	private void GoToHard()
	{
		GameSettings.Mode = GameMode.SinglePlayer;
		GameSettings.Difficulty = AIDifficulty.Hard;

		_clickSound.Play();
		GetTree().ChangeSceneToFile("res://Scenes/Creeper.tscn");
	}
}
