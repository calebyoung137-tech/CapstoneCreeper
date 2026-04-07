using Godot;

public partial class OnlineMultiplayerMenu : Control
{
	private Button _hostButton;
	private Button _joinButton;
	private AudioStreamPlayer _clickSound; // <-- declare the click sound node

	public override void _Ready()
	{
		_hostButton = GetNode<Button>("HBoxContainer/HostButton");
		_joinButton = GetNode<Button>("HBoxContainer/JoinButton");

		// Make sure this node path matches your scene exactly
		_clickSound = GetNode<AudioStreamPlayer>("HBoxContainer/Click");

		_hostButton.Text = "Host Game";
		_joinButton.Text = "Join Game";

		_hostButton.Pressed += OnHost;
		_joinButton.Pressed += OnJoin;
	}

	private void OnHost()
	{
		_clickSound?.Play();

		GetNode<NetworkManager>("/root/Network").HostGame(9999);
		GetTree().ChangeSceneToFile("res://scenes/HostMenu.tscn");
		GD.Print("Host pressed");
	}

	private void OnJoin()
	{
		_clickSound?.Play();

		GetTree().ChangeSceneToFile("res://scenes/JoinMenu.tscn");
	}
}
