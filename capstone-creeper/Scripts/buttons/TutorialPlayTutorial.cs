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
		PackedScene tutorialScene = GD.Load<PackedScene>("res://Scenes/tutorial.tscn");
		//PackedScene tutorialScene = GD.Load<PackedScene>("res://Scenes/game_end.tscn");
		// Instantiate it
		Node tutorialInstance = tutorialScene.Instantiate();

		// Optional: if it's UI, make sure it’s on top by adding to CanvasLayer or at the end of children
		AddChild(tutorialInstance);
	}
   }
