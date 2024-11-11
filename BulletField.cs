using Godot;
using System;
using System.Collections.Generic;

public enum BulletTypes
{
    Default,
    AffectedByGravity
}

public partial class BulletField : Control
{
    Predicate<float[]> deleteConditions;
    Texture2D[] sprites = new Texture2D[] {
        (Texture2D)ResourceLoader.Load("res://Assets/bullet.png"), 
        (Texture2D)ResourceLoader.Load("res://Assets/bullet_2.png")
    };
    Main main;

    public Bullet[] Patterns = new Bullet[] { new Bullet(), new Gravity(), new Bounce(), new Homing(), 
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
            int index = 0;
            foreach (var bullet in ActiveBullets)
            {
                //position.X, position.Y, direction.X, direction.Y, speed, shape, sizeX, sizeY,
                // rotation, 0, script, sprite, r, g, b, a
                if ((int)bullet[10] > 0 && (int)bullet[10] < Patterns.Length)
                {
                    Patterns[(int)bullet[10]].UpdateBullet(ref bullet[0], ref bullet[1], ref bullet[2], ref bullet[3],
                    ref bullet[4], ref bullet[5], ref bullet[6], ref bullet[7], ref bullet[8], ref bullet[9],
                    ref bullet[11], ref bullet[12], ref bullet[13], ref bullet[14], ref bullet[15], ref bullet[16],
                    ref bullet[17], ref bullet[18], ref bullet[19], main.viewportRect, MouseCollider);
                }
                if (PreSpeed(bullet, delta))
                {
                    AddSpeed(bullet, delta);
                }
                bullet[9] += (float)delta;
                index++;
            }
        }

        ActiveBullets.RemoveAll(deleteConditions);
    }

    public override void _Draw()
    {
        Color modulate = new Color();
        Vector2 spriteOffset = new Vector2();
        foreach (var bullet in ActiveBullets)
        {
            modulate.R = bullet[12];
            modulate.G = bullet[13];
            modulate.B = bullet[14];
            modulate.A = bullet[15];
            spriteOffset = sprites[(int)bullet[11]].GetSize() / 2;
            DrawSetTransform(new Vector2(bullet[0], bullet[1]), bullet[8]);
            DrawTexture(sprites[(int)bullet[11]], -spriteOffset, modulate);
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

    public void AddProjectile(Vector2 position, Vector2 direction, float speed, float shape = 0, float sizeX = 10, 
        float sizeY = 10, float rotation = 0, float script = -1, float sprite = 1, float r = 1, float g = 1, 
        float b = 1, float a = 1, float ai1 = 0, float ai2 = 0, float ai3 = 0, float ai4 = 0)
    {

        float[] bullet = new float[] { position.X, position.Y, direction.X, direction.Y, speed, shape, sizeX, sizeY,
        rotation, 0, script, sprite, r, g, b, a, ai1, ai2, ai3, ai4 };

        ActiveBullets.Add(bullet);
    }

    // Check for collisions with individual bullets
    private bool Collide(float[] bullet, Aabb target)
    {
        Aabb bulletAabb = new Aabb(bullet[0] - (bullet[6] / 2), bullet[1] - (bullet[7] / 2), 0, 
            bullet[6], bullet[7], 1);
        if (bulletAabb.Intersects(target))
        {
            if (bullet[5] == 0) return true;
            Vector2 targetCenter = 
                new Vector2(target.Position.X + (target.Size.X / 2), target.Position.Y + (target.Size.X / 2));
            float deltaX = (float)Math.Pow(targetCenter.X - bullet[0], 2);
            float deltaY = (float)Math.Pow(targetCenter.Y - bullet[1], 2);
            float radius = (float)Math.Pow((bullet[6] / 2) + (target.Size.X / 2), 2);
            return Math.Sqrt(deltaX + deltaY) < Math.Sqrt(radius);
        }

        return false;
    }

    // This is where the script asociated with this bullet is run
    private bool PreSpeed(float[] bullet, double delta)
    {

        return true;
    }

	private void AddSpeed(float[] bullet, double delta)
    {
        float finalSpeed = (float)(bullet[4] * delta);
        Vector2 finalDirection = new Vector2(bullet[2], bullet[3]).Normalized();
        bullet[0] += finalDirection.X * finalSpeed;
        bullet[1] += finalDirection.Y * finalSpeed;
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
        return bullet[0] >= GetViewportRect().Size.X + limit || bullet[0] <= -limit ||
                bullet[1] >= GetViewportRect().Size.Y + limit || bullet[1] <= -limit;
    }

    public int GetBulletCount()
    {
        return ActiveBullets.Count;
    }
}
