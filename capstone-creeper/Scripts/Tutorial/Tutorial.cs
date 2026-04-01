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
		overlay.MouseFilter = Control.MouseFilterEnum.Stop;
		AddChild(overlay);
        MoveChild(overlay, 0);// Create tween

        Label paragraph = GetNode<Label>("Paragraph");
		paragraph.Text = tutorialMessages[slideNumber];

		TextureButton next = GetChild<TextureButton>(5);
		TextureButton back = GetChild<TextureButton>(6);
		TextureButton exit = GetChild<TextureButton>(7);
		exit.Pressed += Exit;
		back.Pressed += GoBack;
		next.Pressed += GoNext;

		back.Visible = false; 
		Sprite2D imageHelp = GetNode<Sprite2D>("imageHelp");
        imageHelp.Scale = new Vector2(1.3f, 1.3f);

        imageHelp.Texture = GD.Load<Texture2D>(pathsToImages[slideNumber]);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GoBack()
	{
		slideNumber--; 
		TextureButton back = GetChild<TextureButton>(6);

		if (slideNumber == 0)
		{
			back.Visible = false; 
		}

		TextureButton next = GetChild<TextureButton>(5);
		next.Visible = true;
		Sprite2D imageHelp = GetNode<Sprite2D>("imageHelp");
        imageHelp.Scale = new Vector2(1.3f, 1.3f);
        imageHelp.Texture = GD.Load<Texture2D>(pathsToImages[slideNumber]);
		Label paragraph = GetNode<Label>("Paragraph");
		paragraph.Text = tutorialMessages[slideNumber];


	}
	private void GoNext()
	{
		slideNumber++;
		TextureButton back = GetChild<TextureButton>(6);
		back.Visible = true;

		TextureButton next = GetChild<TextureButton>(5);

		if (slideNumber == 2)
		{
			next.Visible = false;
		}
		Sprite2D imageHelp = GetNode<Sprite2D>("imageHelp");
		imageHelp.Texture = GD.Load<Texture2D>(pathsToImages[slideNumber]);
		imageHelp.Scale = new Vector2(1.3f, 1.3f);
		Label paragraph = GetNode<Label>("Paragraph");
		paragraph.Text = tutorialMessages[slideNumber];

	}
	private void Exit()
	{
		QueueFree();
	}
	string[] tutorialMessages = new string[3]
{
	$"Two rival kingdoms fight to conquer a small island. Each kingdom controls two towers of their own color, on opposite corners of the island. Your goal is to create a continuous path of towers to connect both of your keeps, and establish dominance of the territory. But beware! Diagonal connections don’t count, and your towers can be captured.\n\nEach player starts with 8 knights, with 4 positioned around each keep. On your turn, click a knight to select it. A series of ghosts will appear showing all of its possible moves. Click a ghost to confirm your move, or click the selected knight again to deselect.\n",
    "Knights can make three types of moves: \r\n1. March: click an adjacent space in a cardinal direction to reposition your knight\r\n2. Tower building/capture: click a diagonally adjacent space to hop your knight over a patch of land, placing one of your towers there. If the land contains an opponent's tower, it is destroyed and replaced with one of yours!\r\n3. Joust: click the empty space on the opposite side of an adjacent opponent’s knight to jump over and defeat that knight, removing them from the game permanently.\r\n",
    "The first player to create an unbroken path of towers between their two keeps wins! If either player loses all of their knights, the game is declared a draw. A draw can also be agreed upon following a series of repetitive moves.",
};
	string[] pathsToImages = new string[6]
{
    "res://Assets/TutorialImages/Picture1.png",
    "res://Assets/TutorialImages/Picture2.png",
    "res://Assets/TutorialImages/Picture3.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Purple Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Black Buildings/Barracks.png",
	"res://Assets/Tiny Swords (Free Pack)/Buildings/Red Buildings/Barracks.png",
};
}
