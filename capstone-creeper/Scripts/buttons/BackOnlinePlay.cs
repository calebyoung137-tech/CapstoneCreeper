using Godot;
using System;

public partial class BackOnlinePlay : TextureButton
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
		// I need to find out where the code for the button in the network menu is.
		// Once host has been pressed, leaving the screen requires network cleanup, the same is true for join. 
	var network = GetNode<NetworkManager>("/root/Network"); 
	network.LeaveGame();
	network.Cleanup();
	GetTree().ChangeSceneToFile("res://Scenes/play_menu.tscn");
	}
   }
