using Godot;

public partial class Waiting : CanvasLayer
{
    private Label status;

    public override void _Ready()
    {
        status = GetNode<Label>("statuslabel");
        status.Text = "Waiting for player...";
    }
}
