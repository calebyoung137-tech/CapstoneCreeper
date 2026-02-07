using Godot;
using System.Collections.Generic;
using System.Linq;

public enum MultiplayerRole
{
    None,
    Server,
    Client
}

public partial class NetworkManager : Node
{
    public static NetworkManager Instance { get; private set; }

    public MultiplayerRole Role { get; private set; } = MultiplayerRole.None;
    public ENetMultiplayerPeer peer;
    private HashSet<long> _readyPeers = new HashSet<long>();
    public override void _Ready()
    {
        Instance = this;

        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
    }

    //godot docs
    public Error HostGame(int port = 9999)
    {
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(port, 2); // max 2 players
        if (error != Error.Ok)
        {
            GD.Print("server fail");
            return error;
        }

        Multiplayer.MultiplayerPeer = peer;
        Role = MultiplayerRole.Server;
        _readyPeers.Add(Multiplayer.GetUniqueId());
        return Error.Ok;
    }
    //godot docs
    public Error JoinGame(string address = "127.0.0.1", int port = 9999)
    {
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(address, port);
        if (error != Error.Ok)
        {
            GD.Print("client failed");
            return error;
        }

        Multiplayer.MultiplayerPeer = peer;
        Role = MultiplayerRole.Client;
        return Error.Ok;
    }
    private void OnPeerConnected(long id)
    {
        GD.Print($"Peer connected: {id}");
    }

    private void OnConnectedToServer()
    {
        GD.Print("Connected to server");

        if (Role == MultiplayerRole.Client)
        {
            MarkReady();
        }
    }

    private void OnConnectionFailed()
    {
        GD.Print("Connection failed");
    }

    public void MarkReady()
    {
        if (Role == MultiplayerRole.Client)
        {
                RpcId(1, nameof(ClientReady), Multiplayer.GetUniqueId());
        }
        else if (Role == MultiplayerRole.Server)
        {
            GD.Print("monkey business is afoot");
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ClientReady(long peerId)
    {
        if (!Multiplayer.IsServer())
            return;

        _readyPeers.Add(peerId);

        if (_readyPeers.Count == 2)
        {
          //This should not really have to exist but I could not get this working with one rpc
            StartGameLocal();
            //since there is a local method, only call the rpc on the client
            long clientId = _readyPeers.First(id => id != Multiplayer.GetUniqueId()); 
            RpcId(clientId, nameof(StartGame));
        }
        else
        {
            GD.Print("[Server] Waiting for other player...");
        }
    }

    
    public void StartGameLocal()
    {
        GetTree().ChangeSceneToFile("res://scenes/Creeper.tscn");
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void StartGame()
    {
        GetTree().ChangeSceneToFile("res://scenes/Creeper.tscn");
    }

    public void SendMove(Vector2I from, Vector2I to)
    {
        if (Multiplayer.MultiplayerPeer == null)
        {
            GD.Print("[NetworkManager] Cannot send move: not connected");
            return;
        }

        GD.Print($"[NetworkManager] Sending move {from} -> {to}");
        Rpc(nameof(ReceiveMove), from, to);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ReceiveMove(Vector2I from, Vector2I to)
    {
        var controller = GameController.Controller;
        if (controller != null)
        {
            controller.ApplyRemoteMove(from, to);
            GD.Print($"[NetworkManager] Applied move {from} -> {to}");
        }
        else
        {
            GD.Print("[NetworkManager] GameController is null!");
        }
    }
}
