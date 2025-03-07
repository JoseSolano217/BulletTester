using Godot;
using System;
using System.Collections.Generic;

public enum BulletTypes
{
    Default,
    AffectedByGravity
}

/*position.X, position.Y, direction.X, direction.Y, speed, 0, script, 
            sprite, r, g, b, a, ai1, ai2, ai3*/
public enum BulletComponents
{
    PositionX, 
    PositionY, 
    DirectionX, 
    DirectionY,
    Speed, 
    Time, 
    Type, 
    Sprite, 
    R, 
    G, 
    B, 
    A, 
    Ai1, 
    Ai2, 
    Ai3
}

public partial class BulletField : Control
{
    Predicate<float[]> deleteConditions;
    Texture2D[] sprites = new Texture2D[] {
        (Texture2D)ResourceLoader.Load("res://Assets/bullet.png"), 
        (Texture2D)ResourceLoader.Load("res://Assets/bullet_2.png")
    };
    Main main;

    public Bullet[] Patterns = { new Bullet(), new Gravity(), new Bounce(), new Homing(), 
        new Oscillate(), new Wavy(), new DecayingAngle() };
    public List<float[]> ActiveBullets = new List<float[]>();
    /// <summary>
    /// The Aabb used for mouse collision.
    /// It can be null depending on the user's input.
    /// </summary>
    public Aabb MouseCollider = new Aabb();
    /// <summary>
    /// The list of current aabb colliders.
    /// It starts empty, and Main updates it automatically.
    /// </summary>
    public List<Aabb> Colliders = new List<Aabb>() {
        };

    public override void _Ready()
	{
        deleteConditions = DeleteConditions;
        main = (Main)GetParent();
    }

	public override void _Process(double delta)
	{
        float fps = (float)(60f / ((60f * delta) + 0.0001f));
        QueueRedraw();

        if (!main.paused)
        {
            foreach (var bullet in ActiveBullets)
            {
                if ((int)bullet[(int)BulletComponents.Type] > 0 && 
                    (int)bullet[(int)BulletComponents.Type] < Patterns.Length)
                {
                    Patterns[(int)bullet[(int)BulletComponents.Type]].UpdateBullet(
                        ref bullet[(int)BulletComponents.PositionX], ref bullet[(int)BulletComponents.PositionY], 
                        ref bullet[(int)BulletComponents.DirectionX], 
                        ref bullet[(int)BulletComponents.DirectionY], ref bullet[(int)BulletComponents.Speed], 
                        ref bullet[(int)BulletComponents.Time], ref bullet[(int)BulletComponents.R], 
                        ref bullet[(int)BulletComponents.G], ref bullet[(int)BulletComponents.B], 
                        ref bullet[(int)BulletComponents.A], ref bullet[(int)BulletComponents.Ai1], 
                        ref bullet[(int)BulletComponents.Ai2], ref bullet[(int)BulletComponents.Ai3], 
                        main.viewportRect, MouseCollider);
                }
                AddSpeed(bullet, delta);
                bullet[(int)BulletComponents.Time] += (float)delta;
            }
        }

        ActiveBullets.RemoveAll(deleteConditions);
    }

    public override void _Draw()
    {
        Color modulate = new Color();
        Vector2 spriteOffset = new Vector2();
        float rotation = 0;
        foreach (var bullet in ActiveBullets)
        {
            modulate.R = bullet[(int)BulletComponents.R];
            modulate.G = bullet[(int)BulletComponents.G];
            modulate.B = bullet[(int)BulletComponents.B];
            modulate.A = bullet[(int)BulletComponents.A];
            rotation = new Vector2(bullet[(int)BulletComponents.DirectionX], 
                bullet[(int)BulletComponents.DirectionY]).Angle();
            spriteOffset = sprites[(int)bullet[(int)BulletComponents.Sprite]].GetSize() / 2;
            DrawSetTransform(new Vector2(bullet[(int)BulletComponents.PositionX], 
                bullet[(int)BulletComponents.PositionY]), rotation);
            DrawTexture(sprites[(int)bullet[(int)BulletComponents.Sprite]], -spriteOffset, modulate);
        }
    }

    /// <summary>
    /// Adds a projectile with the given parameters.
    /// </summary>
    /// <param name="centerX">The horizontal alignment of the bullet upon spawning.</param>
    /// <param name="centerY">The vertical alignment of the bullet upon spawning.</param>
    /// <param name="directionX">The horizontal direction of the bullet upon spawning.</param>
    /// <param name="directionY">The vertical direction of the bullet upon spawning.</param>
    /// <param name="speed">The speed of the bullet. It will be multiplied by the current delta, so it should be above 100.</param>
    /// <param name="shape">The shape of the bullet's hitbox, currently, it can only be 0 to be a square or any other value to be a circle.</param>
    /// <param name="sizeX">The width of the bullet's hitbox, it only affects square-shaped bullets.</param>
    /// <param name="sizeY">The height of the bullet's hitbox, it only affects square-shaped bullets.</param>
    /// <param name="rotation">The rotation of the bullet, take in mind this does not affect the hitbox.</param>
    /// <param name="script">The script this bullet uses for it's movement.</param>
    /// <param name="sprite">The sprite drawn for this bullet.</param>
    /// <param name="r">The R channel of this bullet's modulate, a number between 0 and 1.</param>
    /// <param name="g">The G channel of this bullet's modulate, a number between 0 and 1.</param>
    /// <param name="b">The B channel of this bullet's modulate, a number between 0 and 1.</param>
    /// <param name="a">The A channel of this bullet's modulate, a number between 0 and 1.</param>
    /*public void AddProjectile(float centerX, float centerY, float directionX, float directionY, float speed, 
        float shape = 0, float sizeX = 10, float sizeY = 10, float rotation = 0, float script = -1, float sprite = 1, 
        float r = 1, float g = 1, float b = 1, float a = 1)
    {
        float[] bullet = new float[] { centerX, centerY, directionX, directionY, speed, shape, sizeX, sizeY, 
        rotation, 0, script, sprite, r, g, b, a };

        ActiveBullets.Add(bullet);
    }*/

    public void AddProjectile(Vector2 position, Vector2 direction, float speed, float script = -1, 
        float sprite = 1, float r = 1, float g = 1, float b = 1, float a = 1, float ai1 = 0, float ai2 = 0, 
        float ai3 = 0)
    {

        float[] bullet = new float[] { position.X, position.Y, direction.X, direction.Y, speed, 0, script, 
            sprite, r, g, b, a, ai1, ai2, ai3 };

        ActiveBullets.Add(bullet);
    }

    // Check for collisions with individual bullets
    private bool Collide(float[] bullet, Aabb target)
    {
        Aabb bulletAabb = new Aabb(bullet[(int)BulletComponents.PositionX] - 5, 
            bullet[(int)BulletComponents.PositionY] - 5, 0, 10, 10, 1);
        if (bulletAabb.Intersects(target))
        {
            Vector2 targetCenter = 
                new Vector2(target.Position.X + (target.Size.X / 2), target.Position.Y + (target.Size.X / 2));
            float deltaX = (float)Math.Pow(targetCenter.X - bullet[(int)BulletComponents.PositionX], 2);
            float deltaY = (float)Math.Pow(targetCenter.Y - bullet[(int)BulletComponents.PositionY], 2);
            float radius = (float)Math.Pow(5 + (target.Size.X / 2), 2);
            return Math.Sqrt(deltaX + deltaY) < Math.Sqrt(radius);
        }

        return false;
    }

	private void AddSpeed(float[] bullet, double delta)
    {
        float finalSpeed = (float)(bullet[(int)BulletComponents.Speed] * delta);
        Vector2 finalDirection = new Vector2(bullet[(int)BulletComponents.DirectionX], 
            bullet[(int)BulletComponents.DirectionY]).Normalized();
        bullet[(int)BulletComponents.PositionX] += finalDirection.X * finalSpeed;
        bullet[(int)BulletComponents.PositionY] += finalDirection.Y * finalSpeed;
    }

    public void ClearScreen()
    {
        ActiveBullets.Clear();
        Colliders.Clear();
    }

    public bool DeleteConditions(float[] bullet)
    {
        if (Collide(bullet, MouseCollider)) return true;
        foreach (Aabb aabb in Colliders)
        {
            if (Collide(bullet, aabb))
            {
                return true;
            }
        }
        return CheckOutOfBounds(bullet);
    }

    public bool CheckOutOfBounds(float[] bullet)
    {
        int limit = 20;
        return bullet[(int)BulletComponents.PositionX] >= GetViewportRect().Size.X + limit || 
            bullet[(int)BulletComponents.PositionX] <= -limit ||
            bullet[(int)BulletComponents.PositionY] >= GetViewportRect().Size.Y + limit || 
            bullet[(int)BulletComponents.PositionY] <= -limit;
    }

    public int GetBulletCount()
    {
        return ActiveBullets.Count;
    }
}
