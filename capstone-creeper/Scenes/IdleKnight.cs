using Godot;
using System;

public partial class IdleKnight : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite = new AnimatedSprite2D();
		AddChild(animatedSprite);

		// 2️⃣ Load sprite sheet texture
		Texture2D spriteSheet = GD.Load<Texture2D>("res://Assets/Tiny Swords (Free Pack)/Units/Blue Units/Warrior/Warrior_Idle.png");

		// 3️⃣ Create SpriteFrames resource
		var spriteFrames = new SpriteFrames();

		// 4️⃣ Add animation
		string animationName = "idle";
		spriteFrames.AddAnimation(animationName);
		spriteFrames.SetAnimationSpeed(animationName, 10); 

		// 5️⃣ Calculate frame size
		int horizontalFrames = 8;
		int frameWidth = spriteSheet.GetWidth() / horizontalFrames;
		int frameHeight = spriteSheet.GetHeight();

		// 6️⃣ Slice sprite sheet manually
		for (int i = 0; i < horizontalFrames; i++)
		{
			var atlasTexture = new AtlasTexture();
			atlasTexture.Atlas = spriteSheet;
			atlasTexture.Region = new Rect2(
				i * frameWidth,  // x
				0,               // y
				frameWidth,
				frameHeight
			);

			spriteFrames.AddFrame(animationName, atlasTexture);
		}

		// 7️⃣ Assign SpriteFrames
		animatedSprite.SpriteFrames = spriteFrames;

		// 8️⃣ Play animation
		animatedSprite.Play(animationName);
		animatedSprite.Scale = new Vector2(0.7f, 0.7f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
