using Godot;
using System.ComponentModel;

public class Flower : Pattern
{
    [Bindable(true)]
    public float BaseRotation { get; set; } = 0;
    [Bindable(true)]
    public float RotationPerFrame { get; set; } = 0.8f;
    [Bindable(true)]
    public int Petals { get; set; } = 5;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 50;
    [Bindable(true)]
    public bool PointType { get; set; } = true;

    public override bool PreUpdate(Vector2? mousePos)
    {
        if (Timer == 0)
        {
            int spriteType = 0;
            if (!PointType) spriteType = 1;
            float petal = Petals / (Mathf.Pi * 2f);
            Color color = new Color(1, 1, 1, 1);
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

                Vector2 position = new Vector2(width / 2, height / 2);
                float rotation = BaseRotation + i * Mathf.Pi * 2 / NumberOfShots;
                Vector2 direction = new Vector2(1, 0).Rotated(rotation);
                float addedSpeed = 1 + Mathf.Sin((direction.Angle() - BaseRotation) * Petals) * 0.3f;
                CreateSimple(position, direction, 120 * addedSpeed, 1, rotation: rotation, script: Type,
                    sprite: spriteType, r: color.R, g: color.G, b: color.B, a: Alpha, ai1: Ai1, ai2: Ai2, ai3: Ai3, 
                    ai4: 0);
            }
        }
        BaseRotation += RotationPerFrame;
        return base.PreUpdate(mousePos);
    }
}
