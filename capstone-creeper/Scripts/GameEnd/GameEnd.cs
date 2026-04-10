using Godot;
using System;
using static Godot.Control;

public partial class GameEnd : CanvasLayer
{
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

        // Determine the result based on GameController.LastGameEndReason
        string resultNode = GetResultNode(GameController.LastGameEndReason);

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

    private string GetResultNode(GameEndReason reason)
    {
        // Map GameEndReason to the corresponding node name
        return reason switch
        {
            GameEndReason.HostWin => "RedSword",
            GameEndReason.ClientWin => "BlueSword",
            GameEndReason.Draw => "PurpleSword",
            _ => "BlackSword", // Default case
        };
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
