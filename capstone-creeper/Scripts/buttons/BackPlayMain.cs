using Godot;
using System;

public partial class BackPlayMain : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += goToMain; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void goToMain()
	{
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
   }
