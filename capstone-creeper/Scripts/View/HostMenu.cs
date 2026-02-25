using Godot;
using System;

public partial class HostMenu : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
   [Export] private Label ip;
	public override void _Ready()
	{
		ip = GetNode<Label>("IPLabel");
		GetNode<NetworkManager>("/root/Network").HostStarted+=OnHostStarted;
		if (!string.IsNullOrEmpty(GetNode<NetworkManager>("/root/Network").HostIp)) {
			OnHostStarted(GetNode<NetworkManager>("/root/Network").HostIp);
		}
	}
	public void OnHostStarted(string ipAddress) { 
		ip.Text= "Hosting at: " + ipAddress;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
