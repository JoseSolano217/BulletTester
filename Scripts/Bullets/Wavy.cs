﻿using Godot;

public class Wavy : Bullet
{
    Vector2 direction = Vector2.Zero;
    float timeAdjusted = 0;
    public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX,
        ref float directionY, ref float speed, ref float shape, ref float sizeX, ref float sizeY, ref float rotation,
        ref float time, ref float sprite, ref float r, ref float g, ref float b, ref float a, ref float ai1,
        ref float ai2, ref float ai3, ref float ai4, Rect2 viewport, Aabb mouse)
    {
        if (ai1 <= 0) return;
        timeAdjusted = time * ai1;
        direction.X = directionX;
        direction.Y = directionY;
        float sine = Mathf.Sin(timeAdjusted);
        direction = direction.Rotated(sine * ai2 * (1 - Mathf.Abs(sine)));
        directionX = direction.X;
        directionY = direction.Y;
        if (sprite == 0)
        {
            rotation = new Vector2(directionX, directionY).Angle();
        }
    }
}
