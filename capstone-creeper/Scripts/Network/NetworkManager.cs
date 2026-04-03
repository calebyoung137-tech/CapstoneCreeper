using Godot;
using System.Collections.Generic;
using System.Linq;

public enum MultiplayerRole
{
	None,
	Server,
	Client
}

/// <summary>
/// The network manager includes methods to join/host a game, as well as methods that manage 
/// the waiting for a peer to join, and sending moves over the network.
/// TODO: this needs to include logic to handle unexpected endings, disconnections etc. 
/// TODO: decide how to handle pressing join when no server is initiated? 
/// </summary>
public partial class NetworkManager : Node
{
	public MultiplayerRole Role { get; private set; } = MultiplayerRole.None;
	public ENetMultiplayerPeer peer;
	public string HostIp;
	private int _playersLoaded = 0;
	private bool _cleanedUp = false;

	public bool connectedToHost = false;
	//private UdpServer discoveryServer;
	//private PacketPeerUdp discoveryPeer;

	//private const int DiscoveryPort = 9998;

	public override void _Ready()
	{
		//Instantiate Network manager
		// set the peerconnected, connectedtoserver, and Connection failed properties of multiplayer
		// The on connected to server is used to set the game into progress once a player has connected

		Multiplayer.ServerDisconnected += ServerDisconnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.ConnectedToServer += OnConnectedToServer;
		Multiplayer.ConnectionFailed += OnConnectionFailed;
	}

	public override void _Process(double delta)
	{
		if (Role == MultiplayerRole.Client && connectedToHost && Multiplayer.MultiplayerPeer != null)
		{
			if (Multiplayer.MultiplayerPeer == null)
				return;

			var status = Multiplayer.MultiplayerPeer.GetConnectionStatus();

			if (status != MultiplayerPeer.ConnectionStatus.Connected)
			{
				GD.Print("Lost connection to host!");
				Cleanup();
				GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
			}
		}
	}

	public void LeaveGame()
	{
		if (Multiplayer.MultiplayerPeer != null)
		{
			Multiplayer.MultiplayerPeer.Close();
			Multiplayer.MultiplayerPeer = null;
		}

		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
	public Error HostGame(int port = 9999)
	{ // from godot docs, This code established a server, or returns an error
		var addresses = IP.GetLocalAddresses();
		//this logic for the hostip works in the computer lab, not laptops
		HostIp = addresses.Where(ip => ip.Contains(".") && ip.StartsWith("10")).FirstOrDefault();
		if (string.IsNullOrEmpty(HostIp))
		{
			GD.Print("Default to local host when no ip found.");
			HostIp = "127.0.0.1";
		}
		GD.Print(HostIp);
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(port, 2);
		if (error != Error.Ok)
		{
			GD.Print("server fail");
			return error;
		}

		Multiplayer.MultiplayerPeer = peer;
		Role = MultiplayerRole.Server;


		return Error.Ok;
	}
	public Error JoinGame(string address, int port = 9999)
	{  //from godot docs, this joins the server, and creates a client
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateClient(address, port, 0, 0, 2);
		if (error != Error.Ok)
		{
			GD.Print("client failed");
			return error;
		}
		GD.Print("joining game at: " + address);
		// peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		Role = MultiplayerRole.Client;
		return Error.Ok;
	}
	private void Cleanup()
	{
		if (_cleanedUp)
			return;

		_cleanedUp = true;

		GD.Print("Cleaning up network");

		if (Multiplayer != null && Multiplayer.MultiplayerPeer != null)
		{
			Multiplayer.MultiplayerPeer.Close();
			Multiplayer.MultiplayerPeer = null;
		}

		if (peer != null)
		{
			peer.Dispose();
			peer = null;
		}

		Role = MultiplayerRole.None;
		connectedToHost = false;
	}
	private void PeerDisconnected(long id)
	{
		GD.Print("Player disconnected: " + id.ToString());
		Cleanup();
		if (IsInsideTree()) // for whatever reason, this is being executed in some instances where the tree has been disposed of ?
			GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
	private void ServerDisconnected()
	{

		GD.Print("SERVER DISCONNECTED");
		Cleanup();
		//should be an error screen
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}
	private void OnPeerConnected(long id)
	{

		GD.Print($"Peer connected");
	}

	private void OnConnectedToServer()
	{
		connectedToHost = true;
		GD.Print("Connected to server");
		// tell host to call rpc and start the game


		if (Role == MultiplayerRole.Client)
		{
			RpcId(1, nameof(ClientReady), Multiplayer.GetUniqueId());
		}
	}

	private void OnConnectionFailed()
	{
		GD.Print("Connection failed");
	}


	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ClientReady(long peerId)
	{ // this method changes the game scene to game board, only after the peer connects to the server
	  // This has to be called by the server, putting the function calls in the "onconnectedtoserver"
	  // method doesn't start the game for the host for some reason. 
        if (!Multiplayer.IsServer())
            return;

        LoadGameLocal();
        RpcId(peerId, nameof(LoadGame));
    }

    public void LoadGameLocal()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Creeper.tscn");
    }

    //from godot docs
    [Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void LoadGame()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Creeper.tscn");
    }
    public void SendMove(Vector2I from, Vector2I to)
    {
        Rpc(nameof(ReceiveMove), from, to);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ReceiveMove(Vector2I from, Vector2I to)
    {// call this on peer, controller handles logic to apply move locally
        var controller = GameController.Controller;

        controller.ApplyMove(from, to);

        if (controller.IsGameOver())
        {
            LeaveGame();
        }

    }

}
