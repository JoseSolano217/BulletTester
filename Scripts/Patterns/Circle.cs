using Godot;
using System.ComponentModel;

public class Circle : Pattern
{
    [Bindable(true)]
    public float BaseRotation { get; set; } = 0;
    [Bindable(true)]
    public float RotationPerFrame { get; set; } = 0.1f;
    [Bindable(true)]
    public float Disrotation { get; set; } = 0;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 25;

    public override void SetDefaults()
    {
        base.SetDefaults();
    }

    public override bool PreUpdate(Vector2? mousePos, Vector2 position)
    {
        if (Timer == 0)
        {
            if (Cycle == 0)
            {
                BaseRotation -= Disrotation;
            }
            Color color = new Color(1, 1, 1);
            for (int i = 0; i < NumberOfShots; i++)
            {
                float rotation = BaseRotation + i * Mathf.Pi * 2 / NumberOfShots;
                Vector2 direction = new Vector2(1, 0).Rotated(rotation);
                if (Cycle %2 == 0)
                {
                    if (MainPalette >= 0 && MainPalette < palette.Length)
                    {
                        color = palette[MainPalette][random.RandiRange(0, palette[MainPalette].Count-1)];
                    }
                } else
                {
                    if (SecondaryPalette >= 0 && SecondaryPalette < palette.Length)
                    {
                        color = palette[SecondaryPalette][random.RandiRange(0, palette[SecondaryPalette].Count - 1)];
                    }
                }

                CreateSimple(position, direction, 300, Type, SpriteType, color.R, color.G, color.B, Alpha, 
                    ai1: Ai1, ai2: Ai2, ai3: Ai3);
            }
        }
        BaseRotation += RotationPerFrame;
        return base.PreUpdate(mousePos, position);
    }
}
