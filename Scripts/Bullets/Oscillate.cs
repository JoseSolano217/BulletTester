using Godot;

public class Oscillate : Bullet
{
    float timeAdjusted = 0;
    public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX,
        ref float directionY, ref float speed, ref float time, ref float r, ref float g, ref float b, ref float a,
        ref float ai1, ref float ai2, ref float ai3, Rect2 viewport, Aabb mouse)
    {
        if (ai1 <= 0 || ai2 <= 0 || ai3 <= 0) return;
        timeAdjusted = time * ai2;
        if (timeAdjusted <= Mathf.Pi*ai1)
        {
            speed = ai3 * Mathf.Cos(timeAdjusted);
        }
    }
}
