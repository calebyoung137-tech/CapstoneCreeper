using Godot;

public partial class MainMenu : Control
{

    
    private Button _playButton;
    //private Button _helpButton;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("VBoxContainer/PlayButton");
        //_helpButton = GetNode<Button>("VBoxContainer/HelpButton");

        _playButton.Text = "Start";
        //_helpButton.Text = "Help";

        _playButton.Pressed += OnPlayPressed;
        //_helpButton.Pressed += OnHelpPressed;

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

        // Hover text color

        _playButton.AddThemeColorOverride("font_hover_color", new Color(1, 1, 0)); // yellow

        _playButton.AddThemeFontSizeOverride("font_size", 122);

    }

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/play_menu.tscn");
    }

    /*private void OnHelpPressed()
    {
        GD.Print("Help pressed");
        // Later: show help popup or load help scene
    }*/
}
