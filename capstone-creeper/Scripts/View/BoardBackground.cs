using Godot;
using System;

public partial class BoardBackground : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = Vector2.Zero;
		
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				Sprite2D grassTile = new Sprite2D();
				grassTile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Terrain/Tileset/Tilemap_color1.png");
				grassTile.RegionEnabled = true;

				// Set the region rectangle
				grassTile.RegionRect = new Rect2(0f, 0f, 192f, 192f);
				grassTile.Scale = new Vector2(0.52f, 0.52f); 
				grassTile.Position = new Vector2(i * 98, j * 98);
				grassTile.ZIndex = -2; 
				AddChild(grassTile);
			}
		}

		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				if (!(i == 0 && j == 0 || i == 0 && j == 6 || i == 6 && j == 0 || i == 6 && j == 6))
				{
					Sprite2D grassTile = new Sprite2D();
					grassTile.Texture = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Terrain/Tileset/Tilemap_color5.png");
					grassTile.RegionEnabled = true;

					// Set the region rectangle
					grassTile.RegionRect = new Rect2(0f, 0f, 192f, 192f);
					grassTile.Scale = new Vector2(0.27f, 0.27f);
					grassTile.RotationDegrees = 45;
					grassTile.Position = new Vector2((i * 98) - 49, (j * 98) - 49);
					grassTile.ZIndex = -1;
					AddChild(grassTile);
				}
			}
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
