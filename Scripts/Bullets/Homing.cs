using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Homing : Bullet
{
    // By setting this, we can simplify the direction as a Vector2 while not creating a new Vector2 every time
    // UpdateBullet is called
    Vector2 direction = Vector2.Zero;
    Vector2 position = Vector2.Zero;
    Vector2 mousePos = Vector2.Zero;
    Vector2 directionTo = Vector2.Zero;
    public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX,
        ref float directionY, ref float speed, ref float shape, ref float sizeX, ref float sizeY,
        ref float rotation, ref float time, ref float sprite, ref float r, ref float g, ref float b, ref float a,
        ref float ai1, ref float ai2, ref float ai3, ref float ai4, Rect2 viewport, Aabb mouse)
    {
        direction.X = directionX;
        direction.Y = directionY;
        mousePos.X = mouse.GetCenter().X;
        mousePos.Y = mouse.GetCenter().Y;
        position.X = centerX;
        position.Y = centerY;
        if (mousePos != Vector2.Zero)
        {
            directionTo = position.DirectionTo(mousePos);
            direction = direction.Lerp(directionTo, ai1);
            directionX = direction.X;
            directionY = direction.Y;
            if (sprite == 0)
            {
                rotation = direction.Angle();
            }
        }
    }
}
