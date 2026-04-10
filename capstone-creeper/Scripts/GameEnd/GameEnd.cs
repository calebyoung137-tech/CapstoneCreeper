using Godot;
using System;
using static Godot.Control;

public partial class GameEnd : CanvasLayer
{
	public string result = "";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Here is where you would put a victory sound
		var sound = new AudioStreamPlayer();

		sound.Stream = GD.Load<AudioStream>("res://Assets/Sounds/Victory.wav");
		sound.VolumeDb = -5;

		AddChild(sound);
		sound.Play();

		// Auto cleanup
		sound.Finished += () => sound.QueueFree();

		// Optional: auto-remove sound after playing
		sound.Finished += () => sound.QueueFree();

		var overlay = new ColorRect();
		overlay.Color = new Color(0, 0, 0, 0); // start fully transparent
		overlay.SetAnchorsPreset(LayoutPreset.FullRect);
		overlay.MouseFilter = Control.MouseFilterEnum.Stop;


		AddChild(overlay);

		MoveChild(overlay, 0);// Create tween
		var tween = CreateTween();

		// Fade alpha from 0 → 0.5 over 2 seconds
		tween.TweenProperty(
			overlay,
			"color:a",
			0.4f,
			2.0f
		);
		

		if (result == "")
		{
			result = "BlackSword";
		}
		else if (result == "Red")
		{
			result = "RedSword";
		}
		else if (result == "Blue")
		{
			result = "BlueSword";
		}
		else if (result == "Purple")
		{
			result = "PurpleSword";

		}
		TileMapLayer sword = GetNode<TileMapLayer>(result);
		sword.ZIndex = 100;
		sword.Modulate = new Color(1, 1, 1, 0);
		sword.Position = sword.Position + new Vector2(0, -swordPosChange(result));
		// Create tween
		var tween2 = CreateTween();

		// Fade to fully visible over 2 seconds
		tween2.TweenProperty(
			sword,
			"modulate:a",
			1.0f,
			2.0f
		);



		TileMapLayer home = GetNode<TileMapLayer>("RedBanner");

		home.Modulate = new Color(1, 1, 1, 0);

		// Create tween
		var tween3 = CreateTween();

		// Fade to fully visible over 2 seconds
		tween3.TweenProperty(
			home,
			"modulate:a",
			1.0f,
			2.0f
		);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public int swordPosChange(string gameResult)
	{

		if (gameResult == "BlackSword")
		{
			return 770 + 3 * 285;
		}
		else if (gameResult == "RedSword")
		{
			return 770 + 1 * 285;

		}
		else if (gameResult == "BlueSword")
		{
			return 770 + 0 * 285;


		}
		else if (gameResult == "PurpleSword")
		{
			return 770 + 2 * 285;
		}
		else
		{
			return 0;
		}
	}
}
