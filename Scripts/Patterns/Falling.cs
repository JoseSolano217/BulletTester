
using Godot;
using System;
using System.ComponentModel;

public class Falling : Pattern
{
    [Bindable(true)]
    public bool Top { get; set; } = true;

    [Bindable(true)]
    public float TopRange { get; set; } = 1;
    [Bindable(true)]
    public bool Left { get; set; } = false;

    [Bindable(true)]
    public float LeftRange { get; set; } = 1;
    [Bindable(true)]
    public bool Bottom { get; set; } = false;

    [Bindable(true)]
    public float BottomRange { get; set; } = 1;
    [Bindable(true)]
    public bool Right { get; set; } = false;

    [Bindable(true)]
    public float RightRange { get; set; } = 1;
    [Bindable(true)]
    public bool SetPosition { get; set; } = false;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 10;
    [Bindable(true)]
    public float Speed { get; set; } = 125;

    public override void SetDefaults()
    {
        MaxTimer = 10;
    }


    public override bool PreUpdate(Vector2? mousePos, Vector2 pos)
    {
        if (Timer == 0)
        {
            Color color = new Color(1, 1, 1);
            float widthOver2 = width / 2;
            float heightOver2 = height / 2;
            for (int i = 0; i < NumberOfShots; i++)
            {
                if (Cycle % 2 == 0)
                {
                    if (MainPalette >= 0 && MainPalette < palette.Length)
                    {
                        color = palette[MainPalette][random.RandiRange(0, palette[MainPalette].Count - 1)];
                    }
                }
                else
                {
                    if (SecondaryPalette >= 0 && SecondaryPalette < palette.Length)
                    {
                        color = palette[SecondaryPalette][random.RandiRange(0, palette[SecondaryPalette].Count - 1)];
                    }
                }
                if (Top)
                {
                    Vector2 direction = new Vector2(0, 1);
                    Vector2 position;
                    float newWidth = (width-2) * TopRange;
                    float startPoint = 1 + (widthOver2 - (newWidth / 2));
                    if (SetPosition)
                    {
                        position = new Vector2(startPoint + (i * (newWidth / NumberOfShots)), 1);
                    } else
                    {
                        position = new Vector2(widthOver2 + random.RandfRange(-newWidth/2, newWidth / 2), 1);
                    }

                    CreateSimple(position, direction, Speed, Type, SpriteType, color.R, color.G, color.B, Alpha, 
                        ai1: Ai1, ai2: Ai2, ai3: Ai3);
                }
                if (Left)
                {
                    Vector2 direction = new Vector2(1, 0);
                    Vector2 position;
                    float newHeight = (height-2) * LeftRange;
                    float startPoint = 1 + (heightOver2 - (newHeight / 2));
                    if (SetPosition)
                    {
                        position = new Vector2(1, startPoint + (i * (newHeight / NumberOfShots)));
                    }
                    else
                    {
                        position = new Vector2(1, heightOver2 + random.RandfRange(-newHeight/2, newHeight/2));
                    }

                    CreateSimple(position, direction, Speed, Type, SpriteType, color.R, color.G, color.B, Alpha, 
                        ai1: Ai1, ai2: Ai2, ai3: Ai3);
                }
                if (Bottom)
                {
                    Vector2 direction = new Vector2(0, -1);
                    Vector2 position;
                    float newWidth = (width - 2) * BottomRange;
                    float startPoint = 1 + (widthOver2 - (newWidth / 2));
                    if (SetPosition)
                    {
                        position = new Vector2(startPoint + (i * (newWidth / NumberOfShots)), height - 1);
                    }
                    else
                    {
                        position = new Vector2(widthOver2 + random.RandfRange(-newWidth/2, newWidth / 2), height - 1);
                    }

                    CreateSimple(position, direction, Speed, Type, SpriteType, color.R, color.G, color.B, Alpha, 
                        ai1: Ai1, ai2: Ai2, ai3: Ai3);
                }
                if (Right)
                {
                    Vector2 direction = new Vector2(-1, 0);
                    Vector2 position;
                    float newHeight = (height - 2) * RightRange;
                    float startPoint = 1 + (heightOver2 - (newHeight / 2));
                    if (SetPosition)
                    {
                        position = new Vector2(width - 1, startPoint + (i * (newHeight / NumberOfShots)));
                    }
                    else
                    {
                        position = new Vector2(width - 1, heightOver2 + random.RandfRange(-newHeight/2, newHeight/2));
                    }

                    CreateSimple(position, direction, Speed, Type, SpriteType, color.R, color.G, color.B, Alpha, 
                        ai1: Ai1, ai2: Ai2, ai3: Ai3);
                }
            }
        }
        return base.PreUpdate(mousePos, pos);
    }
}
