using Godot;

public class DecayingAngle : Bullet
{
    public Vector2 direction = Vector2.Zero;
    public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX,
        ref float directionY, ref float speed, ref float time, ref float r, ref float g, ref float b, ref float a,
        ref float ai1, ref float ai2, ref float ai3, Rect2 viewport, Aabb mouse)
    {
        direction.X = directionX;
        direction.Y = directionY;
        direction = direction.Rotated(ai1);
        directionX = direction.X;
        directionY = direction.Y;
        if (ai1 > 0)
        {
            ai1 -= ai2;
            if (ai1 < ai3)
            {
                ai1 = ai3;
            }
        }
        if (ai1 < 0)
        {
            ai1 += ai2;
            if (ai1 > -ai3)
            {
                ai1 = -ai3;
            }
        }
    }
}
