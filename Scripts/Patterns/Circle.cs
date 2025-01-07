using Godot;
using System.ComponentModel;

public class Circle : Pattern
{
    [Bindable(true)]
    public Vector2 Position { get; set; } = Vector2.Zero;
    [Bindable(true)]
    public float BaseRotation { get; set; } = 0;
    [Bindable(true)]
    public float RotationPerFrame { get; set; } = 0.1f;
    [Bindable(true)]
    public float Disrotation { get; set; } = 0;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 25;
    [Bindable(true)]
    public bool PointType { get; set; } = false;

    public override void SetDefaults()
    {
        Position = new Vector2(width / 2, height / 2);
        base.SetDefaults();
    }

    public override bool PreUpdate(Vector2? mousePos)
    {
        if (Timer == 0)
        {
            if (Cycle == 0)
            {
                BaseRotation -= Disrotation;
            }
            int spriteType = 0;
            if (!PointType) spriteType = 1;
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

                CreateSimple(Position, direction, 300, 1, 10, 10, rotation, Type, 
                    spriteType, color.R, color.G, color.B, Alpha, ai1: Ai1, ai2: Ai2, ai3: Ai3, ai4: 0);
            }
        }
        BaseRotation += RotationPerFrame;
        return base.PreUpdate(mousePos);
    }
}
