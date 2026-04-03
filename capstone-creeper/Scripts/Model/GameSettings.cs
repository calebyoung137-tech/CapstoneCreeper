using Godot;

public enum GameMode
{
    AIVAI,
    SinglePlayer,
    LocalMultiplayer,
    OnlineMultiplayer
}

public enum AIDifficulty
{
    Easy,
    Hard
}

public partial class GameSettings : Node
{
    public static GameMode Mode = GameMode.SinglePlayer;
    public static AIDifficulty Difficulty = AIDifficulty.Hard;
}
