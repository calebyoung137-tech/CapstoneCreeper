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
    // This will store the currently selected game mode
    public static GameMode Mode = GameMode.SinglePlayer;
}
