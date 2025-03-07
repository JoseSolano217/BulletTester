using Godot;
public class Bounce : Bullet
{
    public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX,
        ref float directionY, ref float speed, ref float time, ref float r, ref float g, ref float b, ref float a,
        ref float ai1, ref float ai2, ref float ai3, Rect2 viewport, Aabb mouse)
    {
        if (ai1 > 0)
        {
            if (centerX <= 0)
            {
                directionX *= -1;
                centerX = 1;
                ai1--;
            }
            if (centerY <= 0)
            {
                directionY *= -1;
                centerY = 1;
                ai1--;
            }
            if (centerX >= viewport.Size.X)
            {
                directionX *= -1;
                centerX = viewport.Size.X - 1;
                ai1--;
            }
            if (centerY >= viewport.Size.Y)
            {
                directionY *= -1;
                centerY = viewport.Size.Y - 1;
                ai1--;
            }
        }
    }
}
