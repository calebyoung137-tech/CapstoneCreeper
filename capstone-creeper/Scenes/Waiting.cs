using Godot;

public partial class Waiting : CanvasLayer
{
    private Label status;

    public override void _Ready()
    {
        status = GetNode<Label>("statuslabel");
        status.Text = "Waiting for player...";

        // Client marks ready immediately
        if (NetworkManager.Instance != null && NetworkManager.Instance.Role == MultiplayerRole.Client)
        {
            NetworkManager.Instance.MarkReady();
        }
    }

    public void SetStatus(string message)
    {
        if (status != null)
            status.Text = message;
    }
}
