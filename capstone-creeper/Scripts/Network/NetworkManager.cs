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
    
    public static NetworkManager Instance { get; private set; }
    public MultiplayerRole Role { get; private set; } = MultiplayerRole.None;
    public ENetMultiplayerPeer peer;
    public string HostIp;
    private int _playersLoaded = 0;
    [Signal]
    public delegate void HostStartedEventHandler(string ipAddress);
    public override void _Ready()
    {
        //Instantiate Network manager
        // set the peerconnected, connectedtoserver, and Connection failed properties of multiplayer
        // The on connected to server is used to set the game into progress once a player has connected
        Instance = this;
        Multiplayer.ServerDisconnected += ServerDisconnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
    }

    public void LeaveGame() {
        if (Multiplayer.MultiplayerPeer != null)
        {
            Multiplayer.MultiplayerPeer.Close();
            Multiplayer.MultiplayerPeer = null;
        }

        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
    public Error HostGame(int port = 9999)
    { // from godot docs, This code established a server, or returns an error
        var addresses = IP.GetLocalAddresses();
        HostIp= addresses.Where(ip=>ip.Contains(".") && ip.StartsWith("10")).First();
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

        if (HostIp != null)
        {
            EmitSignal(SignalName.HostStarted, HostIp);
        }
        return Error.Ok;
    }
    public Error JoinGame(string address, int port = 9999)
    {  //from godot docs, this joins the server, and creates a client
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(address, port,0,0,2);
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
    private void PeerDisconnected(long id)
    { // method from team 8 spring 2025
        GD.Print("Player disconnected: " + id.ToString());
        Multiplayer.MultiplayerPeer = null;
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
    private void ServerDisconnected()
    { // method from team 8 spring 2025
        if (peer != null)
        {
            peer.Close();
            peer = null;
        }
        GD.Print("SERVER DISCONNECTED");
        Multiplayer.MultiplayerPeer = null;
        //probably should be an error screen
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
    private void OnPeerConnected(long id)
    {
        
        GD.Print($"Peer connected");
    }

    private void OnConnectedToServer()
    {
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
        GetTree().ChangeSceneToFile("res://scenes/Creeper.tscn");
    }
    public void SendMove(Vector2I from, Vector2I to)
    { 
        Rpc(nameof(ReceiveMove), from, to);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ReceiveMove(Vector2I from, Vector2I to)
    {// call this on peer, controller handles logic to apply move locally
        var controller = GameController.Controller;
        // the losing side gets to make one additional move before they know the game is over. 
        if (controller.GameOver(from, to))
        {
            LeaveGame();
        }
        else // keep on going
            controller.ApplyMove(from, to);
        
    }
    
}
