using Godot;
using System.ComponentModel;

public class Aimed : Pattern
{
    [Bindable(true)]
    public float MaxAngle { get; set; } = 45;
    [Bindable(true)]
    public int NumberOfShots { get; set; } = 5;

    public override void SetDefaults()
    {
        base.SetDefaults();
    }

    public override bool PreUpdate(Vector2? mousePos, Vector2 position)
    {
        if (Timer == 0)
        {
            Vector2 baseDirection = new Vector2(0, 1);
            if (mousePos != null)
            {
                baseDirection = position.DirectionTo((Vector2)mousePos);
            }

            Color color = new Color(1, 1, 1);
            float halfShotNumber = (float)NumberOfShots / 2f;
            float radianMaxAngle = Mathf.DegToRad(MaxAngle);
            float halfMaxAngle = radianMaxAngle / 2f;
            float rotationPerBullet = radianMaxAngle / NumberOfShots;
            float halfRotation = rotationPerBullet / 2f;
            for (int i = 0; i < NumberOfShots; i++)
            {
                Vector2 direction = baseDirection.Rotated(halfRotation - halfMaxAngle + (rotationPerBullet * i));

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

                CreateSimple(position, direction, 300, Type, SpriteType, color.R, color.G, color.B, Alpha,
                    ai1: Ai1, ai2: Ai2, ai3: Ai3);
            }
        }
        return base.PreUpdate(mousePos, position);
    }
}
