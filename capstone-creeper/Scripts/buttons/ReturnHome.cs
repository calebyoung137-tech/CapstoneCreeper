using Godot;
using System;

public partial class ReturnHome : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += GoHome;
		var style = new StyleBoxFlat();
		style.BgColor = new Color(0, 0, 0, 0); // fully transparent
		style.BorderWidthLeft = 0;
		style.BorderWidthRight = 0;
		style.BorderWidthTop = 0;
		style.BorderWidthBottom = 0;

		AddThemeStyleboxOverride("normal", style);
		AddThemeStyleboxOverride("hover", style);
		AddThemeStyleboxOverride("pressed", style);
		AddThemeStyleboxOverride("focus", style);

		AddThemeColorOverride("font_color", new Color(1, 1, 1)); // white
		AddThemeColorOverride("font_hover_color", new Color(1, 1, 0)); // yellow
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void GoHome()
	{
	  
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
}
