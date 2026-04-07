/*
using Godot;
using System;
using System.Collections.Generic;

public partial class MusicManager : Node
{
	private AudioStreamPlayer musicPlayer;
	private string currentTrack = "";

	// Map scenes to tracks
	private Dictionary<string, string> sceneTracks = new Dictionary<string, string>()
	{
		{ "main_menu.tscn", "res://Assets/Sounds/Music/Main Theme.mp3" },
		{ "play_menu.tscn", "res://Assets/Sounds/Music/Menu Music.mp3" },
		{ "Creeper.tscn", "res://Assets/Sounds/Music/Combat Music.mp3" },
		{ "game_end.tscn", "res://Assets/Sounds/Music/Menu Music.mp3" }
	};

	public override void _Ready()
	{
		musicPlayer = new AudioStreamPlayer();
		musicPlayer.Autoplay = false;
		AddChild(musicPlayer);

		// Connect scene change signal
		GetTree().SceneChanged += OnSceneChanged;
	}

	private void OnSceneChanged(Node newScene)
	{
		string sceneName = newScene.Filename.GetFile();
		if (sceneTracks.ContainsKey(sceneName))
		{
			PlayTrack(sceneTracks[sceneName]);
		}
	}

	public void PlayTrack(string path, float volumeDb = -10f)
	{
		if (currentTrack == path) return;

		currentTrack = path;

		// Fade out old music
		if (musicPlayer.Playing)
		{
			var fadeTween = CreateTween();
			fadeTween.TweenProperty(musicPlayer, "volume_db", -80f, 0.5f)
					 .SetTrans(Tween.TransitionType.Sine)
					 .SetEase(Tween.EaseType.InOut)
					 .SetCallback(() =>
					 {
						 musicPlayer.Stream = GD.Load<AudioStream>(path);
						 musicPlayer.VolumeDb = volumeDb;
						 musicPlayer.Play();
					 });
		}
		else
		{
			musicPlayer.Stream = GD.Load<AudioStream>(path);
			musicPlayer.VolumeDb = volumeDb;
			musicPlayer.Play();
		}
	}

	public void StopMusic()
	{
		musicPlayer.Stop();
		currentTrack = "";
	}

	public void ToggleMute(bool mute)
	{
		int bus = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusMute(bus, mute);
	}
}
*/
