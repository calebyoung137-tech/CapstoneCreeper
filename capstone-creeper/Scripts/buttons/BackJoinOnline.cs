using Godot;
using System;

public partial class BackJoinOnline : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += goToOnline; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void goToOnline()
	{
        var network = GetNode<NetworkManager>("/root/Network");
        network.LeaveGame();
        GetTree().ChangeSceneToFile("res://Scenes/online_multiplayer_menu.tscn");
	}
   }
