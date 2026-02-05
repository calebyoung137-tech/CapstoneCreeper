using Godot;
using System;
using System.Net;



public partial class NetworkManager : Node
{
    public MultiplayerRole Role { get; private set; } = MultiplayerRole.None;
    private ENetMultiplayerPeer peer;
    public static NetworkManager Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }

    public void HostGame(int port = 12345)
    {  //method from godot networking docs
        //https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html
        var peer = new ENetMultiplayerPeer();
        peer.CreateServer(port, maxClients: 2);
        Multiplayer.MultiplayerPeer = peer;
    }

    public void JoinGame(string ip, int port = 12345)
    { // method from godot networking docs
        //https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html
        Role = MultiplayerRole.Client;
        var peer = new ENetMultiplayerPeer();
        peer.CreateClient(ip, port);
        Multiplayer.MultiplayerPeer = peer;

    }

    public void SendMove(Vector2I from, Vector2I to)
    { //motivation for using RPCs for this from team 8 2025, code closely follows that teams code
        RpcId(0, nameof(ReceiveMove), from, to);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ReceiveMove(Vector2I from, Vector2I to)
    { // gpt 
        if (GetTree().CurrentScene is Node root)
        {
            var gameController = root.GetNodeOrNull<GameController>("GameController");
            if (gameController != null)
            {
                gameController.ApplyRemoteMove(from, to);
            }
        }
    }
}
