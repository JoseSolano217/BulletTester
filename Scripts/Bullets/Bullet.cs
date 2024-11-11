using Godot;

public class Bullet
{
    //position.X, position.Y, direction.X, direction.Y, speed, shape, sizeX, sizeY,
    //rotation, 0, script, sprite, r, g, b, a
    public virtual void UpdateBullet(ref float centerX, ref float centerY, ref float directionX, ref float directionY, 
        ref float speed, ref float shape, ref float sizeX, ref float sizeY, ref float rotation, ref float time, 
        ref float sprite, ref float r, ref float g, ref float b, ref float a, ref float ai1, ref float ai2,
        ref float ai3, ref float ai4, Rect2 viewport, Aabb mouse)
    {

    }

    public virtual void OnDeath(ref float centerX, ref float centerY, ref float directionX, ref float directionY,
        ref float speed, ref float shape, ref float sizeX, ref float sizeY, ref float rotation, ref float time, 
        ref float sprite, ref float r, ref float g, ref float b, ref float a, ref float ai1, ref float ai2,
        ref float ai3, ref float ai4, Rect2 viewport, Aabb mouse)
    {

    }
}