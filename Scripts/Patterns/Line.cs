using Godot;
using System.ComponentModel;

public class Line : Pattern
{
    [Bindable(true)]
    public Vector2 End { get; set; } = Vector2.Zero;
    [Bindable(true)]
    public bool SideToSide { get; set; } = true;
    [Bindable(true)]
    public bool BeginToEnd { get; set; } = true;
    [Bindable(true)]
    public float BaseRotation { get; set; } = Mathf.Pi / 2;
    [Bindable(true)]
    public float RotationPerFrame { get; set; } = 0f;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 20;
    [Bindable(true)]
    public float Interval { get; set; } = 2f;
    [Bindable(true)]
    public float NormalMaxTimer { get; set; } = 0.1f;

    public override void SetDefaults()
    {
        MaxTimer = 10;
        MaxCycle = 6;
        End = new Vector2(width, height / 2);
        base.SetDefaults();
    }

    public override bool PreUpdate(Vector2? mousePos, Vector2 pos)
    {
        if (Timer == 0)
        {
            Vector2 difference = End - pos;
            if (!BeginToEnd)
            {
                difference = pos - End;
            }
            Vector2 position = pos + (Cycle * difference / MaxCycle);
            if (!BeginToEnd)
            {
                position = End + (Cycle * difference / MaxCycle);
            }
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
                float rotation = BaseRotation + i * Mathf.Pi * 2 / NumberOfShots;
                Vector2 direction = new Vector2(1, 0).Rotated(rotation);

                CreateSimple(position, direction, 300, Type, SpriteType, r: color.R, g: color.G, b: color.B, 
                    a: Alpha, ai1: Ai1, ai2: Ai2, ai3: Ai3);
            }
            if (Cycle >= MaxCycle - 1)
            {
                MaxTimer = Interval;
                if (SideToSide)
                {
                    BeginToEnd = !BeginToEnd;
                }
            }
            else
            {
                MaxTimer = NormalMaxTimer;
            }
        }
        BaseRotation += RotationPerFrame;
        return base.PreUpdate(mousePos, pos);
    }
}
