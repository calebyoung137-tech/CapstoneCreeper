using Godot;

public partial class Exit : TextureButton
{
	private ConfirmationDialog _dialog;

	public override void _Ready()
	{
		// Create the dialog
		_dialog = new ConfirmationDialog();
		_dialog.Title = "Are you sure?";
		_dialog.DialogText = "Do you want to continue?";
		_dialog.Hide();
		AddChild(_dialog); // IMPORTANT: must be in the scene tree
		//var label = _dialog.GetNode<Label>("Label");
		//Slabel.AddThemeFontSizeOverride("font_size", 32);
		_dialog.Size = new Vector2I(600, 300); // width, height
		_dialog.AddThemeFontSizeOverride("large", 100);
		Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		_dialog.PopupCentered();
	}
}
