using Godot;

public partial class MainMenu : Control
{

    // this menu screen is for setting up the networking functionality, it is for testing and should not make the final version
    // This is AI code that should be rewritten or replaced ! 

    private Button _playButton;
    private Button _helpButton;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("VBoxContainer/PlayButton");
        _helpButton = GetNode<Button>("VBoxContainer/HelpButton");

        _playButton.Text = "Play";
        _helpButton.Text = "Help";

        _playButton.Pressed += OnPlayPressed;
        _helpButton.Pressed += OnHelpPressed;
    }

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/play_menu.tscn");
    }

    private void OnHelpPressed()
    {
        GD.Print("Help pressed");
        // Later: show help popup or load help scene
    }
}
