using Godot;
using System;

public partial class MainMusic : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MusicController.ChangeMusic("res://Assets/Sounds/Music/Combat Music.mp3");

	}
	public override void _ExitTree()
	{
		MusicController.ChangeMusic("res://Assets/Sounds/Music/Main Theme.mp3");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
