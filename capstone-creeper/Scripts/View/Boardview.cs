using Godot;
using Godot.Collections;
using Model;
using System;

using static BoardView;

public partial class BoardView : Node2D
{

	public partial class Tile : ColorRect
	{
		public Vector2I GridPos; // e.g., (x, y) in the grid
	}
	public partial class Pin : ColorRect
	{
		public Vector2I GridPos; // e.g., (x, y) in the grid
	}

	public Dictionary<Vector2I, Tile> tiles = new Dictionary<Vector2I, Tile>();
	public Dictionary<Vector2I, Pin> pins = new Dictionary<Vector2I, Pin>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				if (!((i == 0 && j == 0) || (i == 6 && j == 6) || (i == 0 && j == 6) || (i == 6 && j == 0)))
				{
					Pin pin = new Pin();
					pin.GridPos.X = i;
					pin.GridPos.Y = j;
					pins[new Vector2I(i, j)] = pin;
					pin.Size = new Vector2(16, 16);

					pin.Color = new Color(0.2f, 0.6f, 1.0f);
					pin.Position = new Vector2(i * 50, j * 50);
					AddChild(pin);
				}
			}
		}
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				Tile tile = new Tile();
				tile.GridPos.X = i;
				tile.GridPos.Y = j;
				tiles[new Vector2I(i, j)] = tile;
			   
				// Set size and color
				tile.Size = new Vector2(34, 34);


				tile.Color = new Color(0.2f, 0.6f, 1.0f); // light blue

				// Optional: position it
				tile.Position = new Vector2(i * 50 + 16, j * 50 + 16);
				AddChild(tile);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void updateBoard(in GameBoard gameBoard)
	{
		for (int i = 0; i < gameBoard.Pins.GetLength(0); i++)
		{
			for (int j = 0; j < gameBoard.Pins.GetLength(1); j++)
			{
				if (pins.TryGetValue(new Vector2I(i, j), out Pin pin))
				{
					if (gameBoard.Pins[i, j] == PinType.Empty)
					{
						pin.Color = new Color(0.2f, 0.6f, 1.0f); // light blue
					}
					else if (gameBoard.Pins[i, j] == PinType.Black)
					{
						pin.Color = Colors.Black; 
					}
					else if (gameBoard.Pins[i, j] == PinType.White)
					{
						pin.Color = Colors.White; 
					}
				}
			}
		}

		for (int i = 0; i < gameBoard.Tiles.GetLength(0); i++)
		{
			for (int j = 0; j < gameBoard.Tiles.GetLength(1); j++)
			{
				if (tiles.TryGetValue(new Vector2I(i, j), out Tile tile))
				{
					if (gameBoard.Tiles[i, j] == TileType.Empty)
					{
						tile.Color = new Color(0.2f, 0.6f, 1.0f); // light blue
					}
					else if (gameBoard.Tiles[i, j] == TileType.Black)
					{
						tile.Color = Colors.Black;
					}
					else if (gameBoard.Tiles[i, j] == TileType.White)
					{
						tile.Color = Colors.White;
					}
				}
			}
		}
	}
}
