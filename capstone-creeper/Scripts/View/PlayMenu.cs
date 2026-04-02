using Godot;

public partial class PlayMenu : Control
{
	private Button _aiVAIButton;
	private Button _singlePlayerButton;
	private Button _localMultiplayerButton;
	private Button _onlineMultiplayerButton;
	private Button _backButton;

	public override void _Ready()
	{
		_aiVAIButton = GetNode<Button>("HBoxContainer/AIvAIButton");
		_singlePlayerButton = GetNode<Button>("HBoxContainer/SinglePlayerButton");
		_localMultiplayerButton = GetNode<Button>("HBoxContainer/LocalMultiplayerButton");
		_onlineMultiplayerButton = GetNode<Button>("HBoxContainer/OnlineMultiplayerButton");
		_backButton = GetNode<Button>("HBoxContainer/BackButton");

		_aiVAIButton.Text = "AI vs AI";
		_singlePlayerButton.Text = "Single Player";
		_localMultiplayerButton.Text = "Multiplayer";
		_onlineMultiplayerButton.Text = "Online Multiplayer";
		_backButton.Text = "Back";

		_aiVAIButton.Pressed += OnAIVAI;
		_singlePlayerButton.Pressed += OnSinglePlayer;
		_localMultiplayerButton.Pressed += OnLocalMultiplayer;
		_onlineMultiplayerButton.Pressed += OnOnlineMultiplayer;
		_backButton.Pressed += OnBack;
	}

	private void OnAIVAI()
	{
		GameSettings.Mode = GameMode.AIVAI;
		GetTree().ChangeSceneToFile("res://scenes/AIVAI.tscn");
	}

	private void OnSinglePlayer()
	{
		GameSettings.Mode = GameMode.SinglePlayer;
		GetTree().ChangeSceneToFile("res://scenes/Creeper.tscn");
	}

	private void OnLocalMultiplayer()
	{
		GameSettings.Mode = GameMode.LocalMultiplayer;
		GetTree().ChangeSceneToFile("res://scenes/Creeper.tscn");
	}

	private void OnOnlineMultiplayer()
	{
		GameSettings.Mode = GameMode.OnlineMultiplayer;
		GD.Print("Online Multiplayer selected");
		GetTree().ChangeSceneToFile("res://scenes/online_multiplayer_menu.tscn");
	}

	private void OnBack()
	{
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
