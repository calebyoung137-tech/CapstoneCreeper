using Godot;
using Godot.Collections;
using System;
namespace Model;
using static Model.PinType;
public class GameBoard
{
	const int numPins = 7;
	const int numTiles = 6;

	public PinType[,] Pins;
	public TileType[,] Tiles;

	public void InitBoard()
	{
		Pins = new PinType[7, 7]; 
		Tiles = new TileType[6, 6];
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				if (!(i == 0 && j == 0)
					&& !(i == 0 && j == 6)
					&& !(i == 6 && j == 0)
					&& !(i == 6 && j == 6))

				{
					Pins[i, j] = Corner;
				}
				if ((i == 0 && j == 1)
					|| (i == 0 && j == 2)
					|| (i == 1 && j == 0)
					|| (i == 2 && j == 0)
					|| (i == 6 && j == 4)
					|| (i == 6 && j == 5)
					|| (i == 4 && j == 6)
					|| (i == 5 && j == 6)
					)
				{
					Pins[i, j] = Black;
				}
				else if ((i == 0 && j == 4)
					|| (i == 0 && j == 5)
					|| (i == 1 && j == 6)
					|| (i == 2 && j == 6)
					|| (i == 6 && j == 1)
					|| (i == 6 && j == 2)
					|| (i == 4 && j == 0)
					|| (i == 5 && j == 0)
					)
				{
					Pins[i, j] = White;
				}
				else
				{
					Pins[i, j] = Empty;
				}
			}
		}
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if ((i == 0 && j == 0) || (i == 5 && j == 5))
				{
					Tiles[i, j] = TileType.Black;
				}
				else if ((i == 0 && j == 5) || (i == 6 && j == 5))
				{
					Tiles[i, j] = TileType.White;
				}
				else
				{
					Tiles[i, j] = TileType.Empty;
				}
			}
		}
	}
}
