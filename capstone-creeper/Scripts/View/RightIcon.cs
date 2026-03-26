using Godot;
using System;

public partial class RightIcon : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public Sprite2D sunRays;
	public override void _Ready()
	{

		Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Human Avatars/Avatars_01.png");
		Position = new Vector2(1920 - 300, 540);
		Scale = new Vector2(1.85f, 1.85f);
		//FlipH = true; 

		sunRays = new Sprite2D();
		sunRays.Position = new Vector2(4, 4);
		sunRays.Texture = GD.Load<Texture2D>("res://Assets/frame0004.png");
		sunRays.Scale = new Vector2(2.2f, 2.2f);
		sunRays.ZIndex = -1;
		AddChild(sunRays);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		sunRays.Rotate(0.003f);
		if (GameController.Controller.turn != GameController.Turn.White)
		{
			sunRays.Visible = true;
		}
		else
		{
			sunRays.Visible = false;

		}
	}
}
