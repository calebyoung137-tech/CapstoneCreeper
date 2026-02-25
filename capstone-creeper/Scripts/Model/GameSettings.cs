using Godot;

public enum GameMode
{
    AIVAI,
    SinglePlayer,
    LocalMultiplayer,
    OnlineMultiplayer
}

public partial class GameSettings : Node
{
    public static GameMode Mode = GameMode.SinglePlayer;
}
