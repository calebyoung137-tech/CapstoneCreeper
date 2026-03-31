using Godot;
using System;

public partial class rubberDuck : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public float Speed = -30f;

	public override void _Ready()
	{
		Play("swim");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Position += new Vector2(Speed * (float)delta, 0);

		// If it goes off screen, reset to left
		if (Position.X < -100) // 100 = buffer
		{
			Position = new Vector2(GetViewportRect().Size.X + 100, Position.Y);
		}
	}
}
