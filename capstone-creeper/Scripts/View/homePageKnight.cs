using Godot;
using System;

public partial class homePageKnight : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IdleKnight knight = new IdleKnight();
		knight.Scale = new Vector2(2.0f, 2.0f); 
		AddChild(knight);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
