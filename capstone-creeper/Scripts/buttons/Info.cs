using Godot;
using System;
using System.Reflection.Emit;

public partial class Info : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Pressed += OnPressed;
		TileMapLayer paperBackground = GetNode<TileMapLayer>("paperBackground");
		paperBackground.ZIndex = 10; 
		paperBackground.Position = paperBackground.Position + new Vector2(0, -115);
		paperBackground.Visible = false; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPressed()
	{
		TileMapLayer paperBackground = GetNode<TileMapLayer>("paperBackground");
		
		Godot.Label instructionText = paperBackground.GetChild<HBoxContainer>(1).GetChild<Godot.Label>(0); 
		TextureButton Exit = paperBackground.GetChild<TextureButton>(0);
		fadeIn(paperBackground); 

		Exit.Pressed += exitInstruction;
		string turn = ""; 
		if (GameController.Controller.turn == GameController.Turn.White)
		{
			turn = "Red"; 
		}
		else
		{
			turn = "Blue"; 
		}
		if (GameController.Controller.controllerState == GameController.ControllerState.SelectingPin)
		{
			instructionText.Text = $"{turn} must click on a knight to see available moves.";
		}
		else
		{
			instructionText.Text = $"{turn} must click on a shadow-knight to make a move.";
		}

		instructionText.AddThemeFontSizeOverride("font_size", 55);
		instructionText.AddThemeColorOverride("font_color", Colors.Black);

		Button tutorialButton = paperBackground.GetChild<HBoxContainer>(1).GetChild<Button>(1);
		tutorialButton.Pressed += replayTutorial; 


	}
	public void exitInstruction()
	{
		TileMapLayer paperBackground = GetNode<TileMapLayer>("paperBackground");
		paperBackground.Visible = false; 
	}
	public void replayTutorial() {
		// later this should play a tutorial scene
		TileMapLayer paperBackground = GetNode<TileMapLayer>("paperBackground");
		paperBackground.Visible = false;

        PackedScene tutorialScene = GD.Load<PackedScene>("res://Scenes/tutorial.tscn");

        // Instantiate it
        Node tutorialInstance = tutorialScene.Instantiate();

        // Optional: if it's UI, make sure it’s on top by adding to CanvasLayer or at the end of children
        AddChild(tutorialInstance);
    }

	public void fadeIn(TileMapLayer paperBackground)
	{
		paperBackground.Modulate = new Color(1, 1, 1, 0); // fully transparent
		paperBackground.Visible = true; // must be visible to fade

		// Create a Tween
		Tween tween = CreateTween();

		// Animate alpha from 0 -> 1 over 1 second
		tween.TweenProperty(paperBackground, "modulate:a", 1f, 0.3f)
			 .SetTrans(Tween.TransitionType.Quad)
			 .SetEase(Tween.EaseType.InOut);
	}
}
