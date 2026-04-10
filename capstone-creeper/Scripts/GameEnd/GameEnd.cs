using Godot;
using System;
using static Godot.Control;

public partial class GameEnd : CanvasLayer
{
	public string result { get; set; } // Add this property to store the game result

	public override void _Ready()
	{
		// Play victory sound
		var sound = new AudioStreamPlayer();
		sound.Stream = GD.Load<AudioStream>("res://Assets/Sounds/Victory.wav");
		sound.VolumeDb = -5;
		AddChild(sound);
		sound.Play();
		sound.Finished += () => sound.QueueFree();

		// Create overlay
		var overlay = new ColorRect();
		overlay.Color = new Color(0, 0, 0, 0); // Start fully transparent
		overlay.SetAnchorsPreset(LayoutPreset.FullRect);
		overlay.MouseFilter = Control.MouseFilterEnum.Stop;
		AddChild(overlay);
		MoveChild(overlay, 0);

		// Fade overlay
		var tween = CreateTween();
		tween.TweenProperty(overlay, "color:a", 0.4f, 2.0f);

		// Determine the result based on the Result property
		string resultNode = GetResultNode(result);

		if (!string.IsNullOrEmpty(resultNode))
		{
			TileMapLayer sword = GetNode<TileMapLayer>(resultNode);
			sword.ZIndex = 100;
			sword.Modulate = new Color(1, 1, 1, 0);
			sword.Position += new Vector2(0, -swordPosChange(resultNode));

			// Fade in the sword
			var swordTween = CreateTween();
			swordTween.TweenProperty(sword, "modulate:a", 1.0f, 2.0f);
		}
	}
	private string GetResultNode(object result)
	{
		// Handle GameEndReason input
		if (result is GameEndReason reason)
		{
			return reason switch
			{
				GameEndReason.HostWin => "RedSword",
				GameEndReason.ClientWin => "BlueSword",
				GameEndReason.Draw => "PurpleSword",
				_ => "BlackSword",
			};
		}

		// Handle string input
		if (result is string resultString)
		{
			return resultString switch
			{
				"Red" => "RedSword",
				"Blue" => "BlueSword",
				"Purple" => "PurpleSword",
				_ => "BlackSword", // Default case
			};
		}

		// Default case for unsupported input types
		return "BlackSword";
	}
   

	public int swordPosChange(string gameResult)
	{
		// Adjust sword position based on the result
		return gameResult switch
		{
			"BlackSword" => 770 + 3 * 285,
			"RedSword" => 770 + 1 * 285,
			"BlueSword" => 770 + 0 * 285,
			"PurpleSword" => 770 + 2 * 285,
			_ => 0,
		};
	}
}
