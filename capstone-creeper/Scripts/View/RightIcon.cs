using Godot;
using System;

public partial class RightIcon : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Human Avatars/Avatars_06.png");
		Position = new Vector2(1920 - 300, 540);
		Scale = new Vector2(1.7f, 1.7f);
		//FlipH = true; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
