using Godot;

public partial class OnlineMultiplayerMenu : Control
{
	private Button _hostButton;
	private Button _joinButton;

	public override void _Ready()
	{
		_hostButton = GetNode<Button>("HBoxContainer/HostButton");
		_joinButton = GetNode<Button>("HBoxContainer/JoinButton");

		_hostButton.Text = "Host Game";
		_joinButton.Text = "Join Game";

		_hostButton.Pressed += OnHost;
		_joinButton.Pressed += OnJoin;
	}

	private void OnHost()
	{ // go to waiting
		GetNode<NetworkManager>("/root/Network").HostGame(9999);
		GetTree().ChangeSceneToFile("res://scenes/HostMenu.tscn");
		GD.Print("host pressed");
	}
	
	private void OnJoin()
	{ 
		GetTree().ChangeSceneToFile("res://scenes/JoinMenu.tscn");
	}
}
