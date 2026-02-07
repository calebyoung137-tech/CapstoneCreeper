using Godot;

public partial class OnlineMultiplayerMenu : Control
{
    private Button _hostButton;
    private Button _joinButton;
    private Button _backButton;

    public override void _Ready()
    {
        _hostButton = GetNode<Button>("VBoxContainer/HostButton");
        _joinButton = GetNode<Button>("VBoxContainer/JoinButton");
        _backButton = GetNode<Button>("VBoxContainer/BackButton");

        _hostButton.Text = "Host Game";
        _joinButton.Text = "Join Game";
        _backButton.Text = "Back";

        _hostButton.Pressed += OnHost;
        _joinButton.Pressed += OnJoin;
        _backButton.Pressed += OnBack;
    }

    private void OnHost()
    {
        NetworkManager.Instance.HostGame(9999);
        GetTree().ChangeSceneToFile("res://scenes/waiting.tscn");
        GD.Print("host pressed");
    }
    
    private void OnJoin()
    {
        string ip = "127.0.0.1"; // Replace with LineEdit input later
        NetworkManager.Instance.JoinGame(ip, 9999);
        GetTree().ChangeSceneToFile("res://scenes/waiting.tscn");
    }

    private void OnBack()
    {
        
        GetTree().ChangeSceneToFile("res://scenes/play_menu.tscn");
    }
}
