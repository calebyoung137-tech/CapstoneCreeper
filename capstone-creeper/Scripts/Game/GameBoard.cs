using System.Net.NetworkInformation;

public partial class GameBoard
{
  
    public int Width { get; }
    public int Height { get; }
    public Tile[,] Tiles { get; }

    public GameBoard(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Tiles[x, y] = new Tile(TileType.Empty);
    }

    public void SpreadCreeper()
    {
        // Simple spread: each tile increases neighbor tiles slightly
        float[,] newAmounts = new float[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var tile = Tiles[x, y];
                float spread = tile.Amount * 0.25f;

                foreach (var (nx, ny) in Neighbors(x, y))
                    newAmounts[nx, ny] += spread;
            }
        }

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Tiles[x, y].Amount = Math.Clamp(Tiles[x, y].Amount + newAmounts[x, y], 0f, 1f);
    }

    private (int, int)[] Neighbors(int x, int y)
    {
        return new (int, int)[]
        {
            (x-1, y), (x+1, y), (x, y-1), (x, y+1)
        }.Where(n => n.Item1 >= 0 && n.Item1 < Width && n.Item2 >= 0 && n.Item2 < Height)
         .ToArray();
    }
}

 
