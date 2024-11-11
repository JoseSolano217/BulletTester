using Godot;
using System.ComponentModel;

public class MultiplePoints : Pattern
{
    Vector2 centerPosition;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 12;
    [Bindable(true)]
    public int CooldownTimer { get; set; } = 120;
    [Bindable(true)]
    public int ShootInterval { get; set; } = 10;
    [Bindable(true)]
    public int BulletSpeed { get; set; } = 200;
    [Bindable(true)]
    public float CenterOffset { get; set; } = 25;
    [Bindable(true)]
    public float CenterRotationOffset { get; set; } = 0.9f;

    public override void SetDefaults()
    {
        centerPosition = new Vector2(width / 2, height / 2);
    }

    public override bool PreUpdate(Vector2? mousePos)
    {
        if (Cycle == MaxCycle - 1)
        {
            MaxTimer = CooldownTimer;
        }
        else
        {
            MaxTimer = ShootInterval;
        }
        if (Timer == 0)
        {
            Vector2 position = centerPosition + new Vector2(1, 0)
                .Rotated(CenterRotationOffset * (Cycle + 1)) * CenterOffset * (Cycle + 1);
            Vector2 direction = position.DirectionTo(centerPosition);
            Vector2 bulletDirection = direction;
            Color color = new Color(1, 1, 1);
            float division = 1f / NumberOfShots;
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
                bulletDirection = bulletDirection.Rotated(Mathf.Pi * 2 / NumberOfShots);

                CreateSimple(position, bulletDirection, 200, 1, rotation: bulletDirection.Angle(), script: Type,
                    sprite: 0, r: color.R, g: color.G, b: color.B, a: Alpha, ai1: Ai1, ai2: Ai2, ai3: Ai3, ai4: Ai4);
                //CreateSimple(position, bulletDirection, 200, 1,
                //    rotation: bulletDirection.Angle(), sprite: 0, r: color.R, g: color.G, b: color.B);
            }
        }
        return base.PreUpdate(mousePos);
    }
}
