using Godot;

public partial class GameController : Node
{
    private GameBoard _board;
    private TilesView _view;

    public override void _Ready()
    {
        _board = new GameBoard(16, 16); // 16x16 board
        _view = GetNode<TilesView>("TilesView");

        // Seed initial creeper
        _board.Tiles[8, 8].Amount = 1f;

        _view.DrawBoard(_board);
    }

    public override void _Process(double delta)
    {
        // Spread creeper every frame
        _board.SpreadCreeper();
        _view.DrawBoard(_board);
    }
}
