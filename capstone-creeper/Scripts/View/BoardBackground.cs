using Godot;
using System;

public partial class BoardBackground : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				Sprite2D grassTile = new Sprite2D();
				grassTile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Terrain/Tileset/Tilemap_color1.png");
				grassTile.RegionEnabled = true;

				// Set the region rectangle
				grassTile.RegionRect = new Rect2(192.0f, 192.3f, 64f, 64f);
				grassTile.Scale = new Vector2(1.7f, 1.7f); 
				grassTile.Position = new Vector2(i * 104, j * 104);
				grassTile.ZIndex = 1; 
				AddChild(grassTile);
			}
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
