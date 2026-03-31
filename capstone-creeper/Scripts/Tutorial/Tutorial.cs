using Godot;
using System;
using System.Collections.Generic;
using static Godot.Control;

public partial class Tutorial : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public int slideNumber = 0; 

	public override void _Ready()
	{
		var overlay = new ColorRect();
		overlay.Color = new Color(0, 0, 0, 0.5f);
		overlay.SetAnchorsPreset(LayoutPreset.FullRect);
		overlay.ZIndex = -10;
		overlay.MouseFilter = Control.MouseFilterEnum.Ignore;
		AddChild(overlay);

		Label paragraph = GetNode<Label>("Paragraph");
		paragraph.Text = tutorialMessages[slideNumber];

		TextureButton next = GetChild<TextureButton>(4);
		TextureButton back = GetChild<TextureButton>(5);
		TextureButton exit = GetChild<TextureButton>(6);
		exit.Pressed += Exit;
		back.Pressed += GoBack;
		next.Pressed += GoNext;

		back.Visible = false; 
		Sprite2D imageHelp = GetNode<Sprite2D>("imageHelp"); 
		imageHelp.Texture = GD.Load<Texture2D>(pathsToImages[slideNumber]);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GoBack()
	{
		slideNumber--; 
		TextureButton back = GetChild<TextureButton>(5);

		if (slideNumber == 0)
		{
			back.Visible = false; 
		}

		TextureButton next = GetChild<TextureButton>(4);
		next.Visible = true;
		Sprite2D imageHelp = GetNode<Sprite2D>("imageHelp");
		imageHelp.Texture = GD.Load<Texture2D>(pathsToImages[slideNumber]);
		Label paragraph = GetNode<Label>("Paragraph");
		paragraph.Text = tutorialMessages[slideNumber];


	}
	private void GoNext()
	{
		slideNumber++;
		TextureButton back = GetChild<TextureButton>(5);
		back.Visible = true;

		TextureButton next = GetChild<TextureButton>(4);

		if (slideNumber == 5)
		{
			next.Visible = false;
		}
		Sprite2D imageHelp = GetNode<Sprite2D>("imageHelp");
		imageHelp.Texture = GD.Load<Texture2D>(pathsToImages[slideNumber]);
		Label paragraph = GetNode<Label>("Paragraph");
		paragraph.Text = tutorialMessages[slideNumber];

	}
	private void Exit()
	{
		QueueFree();
	}
	string[] tutorialMessages = new string[6]
{
	"Placeholder messagePlaceholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message 1",
	"Placeholder messagePlaceholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message 2",
	"Placeholder messagePlaceholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message 3",
	"Placeholder messagePlaceholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message 4",
	"Placeholder messagePlaceholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message 5",
    "Placeholder messagePlaceholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message Placeholder message 6"
};
	string[] pathsToImages = new string[6]
{
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Black Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Blue Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Purple Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Black Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Barracks.png",
};
}
