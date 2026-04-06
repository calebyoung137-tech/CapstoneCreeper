using Godot;

public partial class Exit : TextureButton
{
	private Control _popup;

	public override void _Ready()
	{
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		// Prevent duplicates
		if (_popup != null)
			return;

		// Root popup container (covers screen)
		_popup = new Control();
		_popup.ZIndex = 10; 
		_popup.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		_popup.SizeFlagsVertical = SizeFlags.ExpandFill;
		_popup.SetAnchorsPreset(LayoutPreset.FullRect);
		GetTree().Root.AddChild(_popup); // ✅ adds it to the top-level scene
		//AddChild(_popup); // Add to this button's parent instead if needed

		// Optional: dark background overlay
		var overlay = new ColorRect();
		overlay.Color = new Color(0, 0, 0, 0.5f);
		overlay.SetAnchorsPreset(LayoutPreset.FullRect);
		_popup.AddChild(overlay);

		// Center container
		var center = new CenterContainer();
		
		center.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		_popup.AddChild(center);

		// Panel (your dialog box)
		var panel = new Panel();
		panel.CustomMinimumSize = new Vector2(800, 300);
		var style = new StyleBoxFlat();
		style.BgColor = new Color(0, 0, 0, 0); // your background color
		
		panel.AddThemeStyleboxOverride("panel", style);
		center.AddChild(panel);

		// VBox layout
		var vbox = new VBoxContainer();
		vbox.SetAnchorsPreset(LayoutPreset.FullRect);
		vbox.OffsetLeft = 20;
		vbox.OffsetRight = -20;
		vbox.OffsetTop = 20;
		vbox.OffsetBottom = -20;
		panel.AddChild(vbox);

		// Label
		var label = new Label();
		label.Text = "Return to main menu?";
		label.AddThemeFontSizeOverride("font_size", 90); 
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.AutowrapMode = TextServer.AutowrapMode.Word;
		vbox.AddChild(label);

		// Spacer
		vbox.AddChild(new Control { CustomMinimumSize = new Vector2(0, 20) });

		// Buttons row
		var hbox = new HBoxContainer();
		hbox.Alignment = BoxContainer.AlignmentMode.Center;
		hbox.AddThemeConstantOverride("separation", 50);
		vbox.AddChild(hbox);

		// YES button
		var yesButton = new Button();
		yesButton.Text = "Yes";

		yesButton.AddThemeFontSizeOverride("font_size", 70);

		yesButton.Pressed += OnConfirmExit;
		var yesButtonStyle = new StyleBoxFlat();
		yesButtonStyle.BgColor = new Color(0, 0, 0, 0); // fully transparent
		yesButtonStyle.SetBorderWidthAll(0); // no border

		// Apply to button
		yesButton.AddThemeStyleboxOverride("normal", yesButtonStyle);
		yesButton.AddThemeStyleboxOverride("hover", yesButtonStyle);
		yesButton.AddThemeStyleboxOverride("pressed", yesButtonStyle);
		yesButton.AddThemeStyleboxOverride("disabled", yesButtonStyle);

		yesButton.AddThemeColorOverride("font_color", Colors.White); // normal color
		yesButton.AddThemeColorOverride("font_hover_color", Colors.Yellow); // hover color
		yesButton.AddThemeColorOverride("font_pressed_color", Colors.Orange); // optional pressed color

		hbox.AddChild(yesButton);

		// NO button
		var noButton = new Button();
		noButton.Text = "No";
		noButton.AddThemeFontSizeOverride("font_size", 70);
		noButton.Pressed += ClosePopup;
		var noButtonStyle = new StyleBoxFlat();
		noButtonStyle.BgColor = new Color(0, 0, 0, 0); // fully transparent
		noButtonStyle.SetBorderWidthAll(0); // no border

		// Apply to button
		noButton.AddThemeStyleboxOverride("normal", noButtonStyle);
		noButton.AddThemeStyleboxOverride("pressed", noButtonStyle);
		noButton.AddThemeStyleboxOverride("disabled", noButtonStyle);
		noButton.AddThemeStyleboxOverride("hover", noButtonStyle);

		noButton.AddThemeColorOverride("font_color", Colors.White); // normal color
		noButton.AddThemeColorOverride("font_hover_color", Colors.Yellow); // hover color
		noButton.AddThemeColorOverride("font_pressed_color", Colors.Orange); // optional pressed color
		
		hbox.AddChild(noButton);


	}

	private void OnConfirmExit()
	{
		if (_popup != null)
		{
			_popup.QueueFree();
			_popup = null;
		}
		if(GetTree().CurrentScene.SceneFilePath== "res://Scenes/Creeper.tscn")
		{
			//these methods should be safe to call even if there is no network game
            var network = GetNode<NetworkManager>("/root/Network");
            network.LeaveGame();
            GetTree().ChangeSceneToFile("res://Scenes/online_multiplayer_menu.tscn");
        }
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}

	private void ClosePopup()
	{
		_popup.QueueFree();
		_popup = null;
	}
}
