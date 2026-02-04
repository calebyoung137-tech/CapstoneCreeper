using Godot;
using System;



public partial class NetworkManager : Node
{
    public MultiplayerRole Role { get; private set; } = MultiplayerRole.None;
    private ENetMultiplayerPeer peer;
    public static NetworkManager Instance { get; private set; }
    public override void _Ready()
    {
        GD.Print("NetworkManager ready");
        Instance = this;
    }

    public void HostGame(int port = 12345)
    {
        peer = new ENetMultiplayerPeer();
        var result = peer.CreateServer(port, maxClients: 2);
        if (result != Error.Ok)
        {
            GD.PrintErr("Failed to start server: ", result);
            return;
        }

        Multiplayer.MultiplayerPeer = peer;
        Role = MultiplayerRole.Server;
        GD.Print("Hosting game on port ", port);
    }

    public void JoinGame(string ip, int port = 12345)
    {
        peer = new ENetMultiplayerPeer();
        var result = peer.CreateClient(ip, port);
        if (result != Error.Ok)
        {
            GD.PrintErr("Failed to connect: ", result);
            return;
        }

        Multiplayer.MultiplayerPeer = peer;
        Role = MultiplayerRole.Client;
        GD.Print("Joined game at ", ip, ":", port);
    }

    public void SendMove(Vector2I from, Vector2I to)
    {
        if (Role == MultiplayerRole.None)
        {
            GD.PrintErr("Not connected");
            return;
        }

        RpcId(0, nameof(ReceiveMove), from, to);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void ReceiveMove(Vector2I from, Vector2I to)
    {
        GD.Print("Received move: ", from, " -> ", to);

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
