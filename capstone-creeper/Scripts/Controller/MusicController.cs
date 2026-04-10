using Godot;

public partial class MusicController : Node
{
    private static AudioStreamPlayer _player;

    public override void _Ready()
    {
        _player = new AudioStreamPlayer();
        AddChild(_player);

        _player.Stream = GD.Load<AudioStream>("res://Assets/Sounds/Music/Main Theme.mp3");
        _player.Autoplay = true;
        _player.VolumeDb = -20;

        //_player.Bus = "Music"; // Make sure this bus exists

        // Looping is set on the audio file itself (see step 3)
        _player.Play();
    }

    public static void ChangeMusic(string path)
    {
        _player.Stream = GD.Load<AudioStream>(path);
        _player.VolumeDb = -20;

        _player.Play();
    }
}
