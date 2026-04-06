using Godot;
using System;

public partial class TutorialPlayTutorial : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += goToTutorial; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void goToTutorial()
	{
		GetTree().ChangeSceneToFile("res://Scenes/tutorial.tscn");
	}
   }
