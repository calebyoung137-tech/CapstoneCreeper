using Godot;

public partial class MainMenu : Control
{
	private Button _playButton;
	private AudioStreamPlayer _clickSound;

	public override void _Ready()
	{
		_playButton = GetNode<Button>("VBoxContainer/PlayButton");
		_clickSound = GetNode<AudioStreamPlayer>("VBoxContainer/Click");

		_playButton.Text = "Start";
		_playButton.Pressed += OnPlayPressed;

		// Button styling
		var style = new StyleBoxFlat();
		style.BgColor = new Color(0, 0, 0, 0); // fully transparent
		style.BorderWidthLeft = 0;
		style.BorderWidthRight = 0;
		style.BorderWidthTop = 0;
		style.BorderWidthBottom = 0;

		_playButton.AddThemeStyleboxOverride("normal", style);
		_playButton.AddThemeStyleboxOverride("hover", style);
		_playButton.AddThemeStyleboxOverride("pressed", style);
		_playButton.AddThemeStyleboxOverride("focus", style);

		_playButton.AddThemeColorOverride("font_color", new Color(1, 1, 1)); // white
		_playButton.AddThemeColorOverride("font_hover_color", new Color(1, 1, 0)); // yellow
		_playButton.AddThemeFontSizeOverride("font_size", 122);
	}

	private void OnPlayPressed()
	{
		// Play click sound
		_clickSound?.Play();

		// Change scene
		GetTree().ChangeSceneToFile("res://scenes/play_menu.tscn");
	}
}
