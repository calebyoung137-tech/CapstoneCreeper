using Godot;

public partial class TilesView : Node2D
{
	[Export] public TileMap TileMap;

	public void DrawBoard(GameBoard board)
	{
		TileMap.Clear();

		for (int x = 0; x < board.Width; x++)
		{
			for (int y = 0; y < board.Height; y++)
			{
				var tile = board.Tiles[x, y];
				if (tile.Amount > 0)
				{
					int tileId = (int)(tile.Amount * (TileMap.TileSet.GetLastUnusedTileId() - 1));
					TileMap.SetCell(0, new Vector2I(x, y), tileId);
				}
			}
		}
	}
}
