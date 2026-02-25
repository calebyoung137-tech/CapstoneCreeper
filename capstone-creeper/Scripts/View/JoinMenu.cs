using Godot;
using System;

public partial class JoinMenu : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	 private LineEdit ipinfo;
	 private Button JoinButton;
	 private Label JoinLabel;

	public override void _Ready()
	{
		ipinfo = GetNode<LineEdit>("JoinContainer/IPINFO");
		
		JoinButton = GetNode<Button>("JoinContainer/Button");
		JoinLabel = GetNode<Label>("JoinContainer/Label");
		JoinLabel.Text = "Enter an IP address to join";
		JoinButton.Text = "Join";
		JoinButton.Pressed += OnJoinPressed;
	}
	private void OnJoinPressed() { 
		string ip= ipinfo.Text.Trim();
		if (string.IsNullOrEmpty(ip))
		{
			GD.Print("empty string");
			return;
		}
		NetworkManager.Instance.JoinGame(ip, 9999);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
