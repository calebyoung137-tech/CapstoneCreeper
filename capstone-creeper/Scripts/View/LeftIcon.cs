using Godot;
using System;

public partial class LeftIcon : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Human Avatars/Avatars_01.png");
		Position = new Vector2(300, 540);
		Scale = new Vector2(1.7f, 1.7f); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
