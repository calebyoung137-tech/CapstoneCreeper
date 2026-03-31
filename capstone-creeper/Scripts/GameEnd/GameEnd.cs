using Godot;
using System;
using static Godot.Control;

public partial class GameEnd : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		var overlay = new ColorRect();
		overlay.Color = new Color(0, 0, 0, 0); // start fully transparent
		overlay.SetAnchorsPreset(LayoutPreset.FullRect);
		overlay.MouseFilter = Control.MouseFilterEnum.Ignore;

		AddChild(overlay);

		// Create tween
		var tween = CreateTween();

		// Fade alpha from 0 → 0.5 over 2 seconds
		tween.TweenProperty(
			overlay,
			"color:a",
			0.4f,
			2.0f
		);


		TileMapLayer sword = GetNode<TileMapLayer>("BlueSword");
		sword.ZIndex = 100; 
		sword.Modulate = new Color(1, 1, 1, 0);
		sword.Position = sword.Position + new Vector2(0, -770); 
		// Create tween
		var tween2 = CreateTween();

		// Fade to fully visible over 2 seconds
		tween2.TweenProperty(
			sword,
			"modulate:a",
			1.0f,
			2.0f
		);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
