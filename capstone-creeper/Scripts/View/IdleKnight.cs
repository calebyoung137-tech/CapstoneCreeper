using Godot;
using System;

public partial class IdleKnight : Node2D
{
  
    // Called when the node enters the scene tree for the first time.
    public void setKnight(string knightColor, AnimatedSprite2D knight)
    {
        // ✅ 1. Choose which knight you want
        // Change to "Red" for red knight
        string spritePath = $"res://Assets/Tiny Swords (Free Pack)/Units/{knightColor} Units/Warrior/Warrior_Idle.png";

        // 2️⃣ Load sprite sheet texture
        Texture2D spriteSheet = GD.Load<Texture2D>(spritePath);

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
                i * frameWidth,
                0,
                frameWidth,
                frameHeight
            );

            spriteFrames.AddFrame(animationName, atlasTexture);
        }

        // 7️⃣ Assign SpriteFrames
        knight.SpriteFrames = spriteFrames;

        // 8️⃣ Play animation
        knight.Play(animationName);
        knight.Scale = new Vector2(0.7f, 0.7f);
    }
    public override void _Ready()
    {
        var animatedSprite = new AnimatedSprite2D();
        AddChild(animatedSprite);

        setKnight("Blue", animatedSprite); 
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
