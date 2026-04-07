using Godot;
using System;

public partial class MusicMute : TextureButton
{
	private bool muted = false;

	public override void _Ready()
	{
		// Connect button press
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		muted = !muted;

		FadeMaster(muted);

		// 🎨 Visual feedback (fade button icon)
		if (muted)
		{
			Modulate = new Color(1, 1, 1, 0.4f); // faded
		}
		else
		{
			Modulate = new Color(1, 1, 1, 1f); // normal
		}
	}

	public void FadeMaster(bool mute)
	{
		int bus = AudioServer.GetBusIndex("Master");
		float target = mute ? -80f : 0f;

		var tween = CreateTween();

		tween.TweenMethod(
			Callable.From<float>((v) => AudioServer.SetBusVolumeDb(bus, v)),
			AudioServer.GetBusVolumeDb(bus),
			target,
			0.5f
		);
	}
}
