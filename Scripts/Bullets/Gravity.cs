using Godot;

public class Gravity : Bullet
{
    public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX,
        ref float directionY, ref float speed, ref float time, ref float r, ref float g, ref float b, ref float a,
        ref float ai1, ref float ai2, ref float ai3, Rect2 viewport, Aabb mouse)
    {
        directionY += 0.0125f;
        directionX *= 0.999f;
    }
}
