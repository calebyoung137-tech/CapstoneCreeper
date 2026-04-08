using Godot;
using System;

public partial class BackTutorialPlay : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += goToPlay; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void goToPlay()
	{
		//GetTree().ChangeSceneToFile("res://Scenes/play_menu.tscn");
	}
   }
