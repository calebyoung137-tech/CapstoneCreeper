using Godot;
using System;
using System.Drawing;

public partial class tree1 : AnimatedSprite2D
{
  
   
    

    
    public override void _Ready()
    {
        
        string spritePath = $"res://Assets/Tiny Swords (Free Pack)/Terrain/Resources/Wood/Trees/Tree1.png";

        // 2️⃣ Load sprite sheet texture
        Texture2D spriteSheet = GD.Load<Texture2D>(spritePath);

        // 3️⃣ Create SpriteFrames resource
        var spriteFrames = new SpriteFrames();

        // 4️⃣ Add animation
        string animationName = "treeSway";
        spriteFrames.AddAnimation(animationName);
        spriteFrames.SetAnimationSpeed(animationName, 6);

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
        SpriteFrames = spriteFrames;
        Scale = new Vector2(0.5f, 0.5f); 
        // 8️⃣ Play animation

        
            Play(animationName);
            
        

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
