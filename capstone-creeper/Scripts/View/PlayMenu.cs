using Godot;

public partial class PlayMenu : Control
{
	private Button _aiVAIButton;
	private Button _singlePlayerButton;
	private Button _localMultiplayerButton;
	private Button _onlineMultiplayerButton;

	private AudioStreamPlayer _clickSound; // <-- declare the node

	public override void _Ready()
	{
		_aiVAIButton = GetNode<Button>("HBoxContainer/AIvAIButton");
		_singlePlayerButton = GetNode<Button>("HBoxContainer/SinglePlayerButton");
		_localMultiplayerButton = GetNode<Button>("HBoxContainer/LocalMultiplayerButton");
		_onlineMultiplayerButton = GetNode<Button>("HBoxContainer/OnlineMultiplayerButton");

		//_clickSound = GetNode<AudioStreamPlayer>("HBoxContainer/Click"); // <-- get the node

		_aiVAIButton.Text = "AI vs AI";
		_singlePlayerButton.Text = "Single Player";
		_localMultiplayerButton.Text = "Multiplayer";
		_onlineMultiplayerButton.Text = "Online Multiplayer";

		_aiVAIButton.Pressed += OnAIVAI;
		_singlePlayerButton.Pressed += OnSinglePlayer;
		_localMultiplayerButton.Pressed += OnLocalMultiplayer;
		_onlineMultiplayerButton.Pressed += OnOnlineMultiplayer;
	}

	// Helper method to play the click sound from the node
	private void PlayClickSound()
	{
		//_clickSound?.Play();
	}

	private void OnAIVAI()
	{
		PlayClickSound();
		GameSettings.Mode = GameMode.AIVAI;
		GetTree().ChangeSceneToFile("res://scenes/AIVAI.tscn");
	}

	private void OnSinglePlayer()
	{
		PlayClickSound();
		GetTree().ChangeSceneToFile("res://Scenes/AIDifficultySelect.tscn");
	}

	private void OnLocalMultiplayer()
	{
		PlayClickSound();
		GameSettings.Mode = GameMode.LocalMultiplayer;
		GetTree().ChangeSceneToFile("res://scenes/Creeper.tscn");
	}

	private void OnOnlineMultiplayer()
	{
		PlayClickSound();
		GameSettings.Mode = GameMode.OnlineMultiplayer;
		GD.Print("Online Multiplayer selected");
		GetTree().ChangeSceneToFile("res://scenes/online_multiplayer_menu.tscn");
	}
}	
