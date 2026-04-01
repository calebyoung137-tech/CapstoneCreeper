using Godot;
using System;

public partial class Clouds03 : Sprite2D
{
	[Export] public float Speed = 15f;

	public override void _Process(double delta)
	{
		// Move to the right
		Position += new Vector2(Speed * (float)delta, 0);

		// If it goes off screen, reset to left
		if (Position.X > GetViewportRect().Size.X + 100) // 100 = buffer
		{
			Position = new Vector2(-100, Position.Y);
		}
	}
}
