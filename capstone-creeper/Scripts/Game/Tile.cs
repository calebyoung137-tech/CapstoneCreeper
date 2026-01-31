using System;


public enum TileType { Empty, Creeper }

public class Tile
{
    public TileType Type { get; set; }
    public float Amount { get; set; } // 0-1, represents creeper density

    public Tile(TileType type)
    {
        Type = type;
        Amount = 0f;
    }
}

