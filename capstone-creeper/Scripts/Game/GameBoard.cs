using System.Net.NetworkInformation;

public partial class GameBoard
{
    public Tile[,] TileBoard = new Tile[6, 6];
    public Pin[,] PinBoard = new Pin[7, 7];

    public Tile GetTile(int x, int y) => TileBoard[x, y];
    public void SetTile(int x, int y, Tile tile) => TileBoard[x, y] = tile;

    public Pin GetPin(int x, int y) => PinBoard[x, y];
    public void SetPin(int x, int y, Pin pin) => PinBoard[x, y] = pin;
}
 
