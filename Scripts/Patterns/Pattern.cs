using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Pattern
{
    public RandomNumberGenerator random = new RandomNumberGenerator();
    // Easy (Blue/Green)
    public List<Color>[] palette = { new List<Color>() {
        new Color(0.35f, 0.45f, 0.85f), new Color(0.35f, 0.8f, 0.475f)
    // Normal (Yellow/Green)
    }, new List<Color>() {
        new Color(0.8f, 0.85f, 0.4f), new Color(0.35f, 0.8f, 0.475f)
    // Hard (Yellow/Red)
    }, new List<Color>() {
        new Color(0.8f, 0.85f, 0.4f), new Color(0.9f, 0.3f, 0.3f)
    // Anomalous (Purple/Red)
    }, new List<Color>() {
        new Color(0.55f, 0.2f, 0.55f), new Color(0.9f, 0.3f, 0.3f)
    // Monochrome (Black/White)
    }, new List<Color>() {
        new Color(0f, 0f, 0f), new Color(1f, 1f, 1f)
    // Temperature (Red/Blue)
    }, new List<Color>() {
        new Color(1f, 0.4f, 0.4f), new Color(0.5f, 0.5f, 1f)
    }};
    [Bindable(true)]
    public float Alpha { get; set; } = 1f;
    [Bindable(true)]
    public int MainPalette { get; set; } = -1;
    [Bindable(true)]
    public int SecondaryPalette { get; set; } = -1;

    public delegate void BulletCreate(float centerX, float centerY, float directionX, float directionY,
        float speed, float shape = 0, float sizeX = 10, float sizeY = 10, float rotation = 0, float script = -1,
        float sprite = 1, float r = 1, float g = 1, float b = 1, float a = 1, float ai1 = 0, float ai2 = 0, 
        float ai3 = 0, float ai4 = 0);
    public delegate void SimplifiedBulletCreate(Vector2 position, Vector2 direction, float speed, 
        float shape = 0, float sizeX = 10, float sizeY = 10, float rotation = 0, float script = -1, float sprite = 1, 
        float r = 1, float g = 1, float b = 1, float a = 1, float ai1 = 0, float ai2 = 0, float ai3 = 0, 
        float ai4 = 0);

    public BulletCreate Create { get; set; }
    public SimplifiedBulletCreate CreateSimple { get; set; }

    public float width = 0;
    public float height = 0;
    [Bindable(true)]
    public float Timer { get; set; } = 0;
    [Bindable(true)]
    public float MaxTimer { get; set; } = 1;
    [Bindable(true)]
    public int Cycle { get; set; } = 0;
    [Bindable(true)]
    public int MaxCycle { get; set; } = 10;
    [Bindable(true)]
    public float Type { get; set; } = 0;
    [Bindable(true)]
    public float Ai1 { get; set; } = 0;
    [Bindable(true)]
    public float Ai2 { get; set; } = 0;
    [Bindable(true)]
    public float Ai3 { get; set; } = 0;

    public virtual void SetDefaults()
    {
    }

    public virtual bool PreUpdate(Vector2? mousePos)
    {
        return true;
    }

    public virtual void Update(Vector2? mousePos, double delta)
    {
        if (!PreUpdate(mousePos)) return;
        Timer += (float)delta;
        if (Timer >= MaxTimer)
        {
            Cycle++;
            Timer = 0;
        }
        if (Cycle >= MaxCycle) Cycle = 0;
        PostUpdate(mousePos);
    }

    public virtual void PostUpdate(Vector2? mousePos)
    {
    }

    public void SetSize(float width, float height)
    {
        this.width = width;
        this.height = height;
    }

    //Func<float, float, float, float, float, float, float, float, float, float, float, float, float,
    //float, float, float>
    // Action<float, float, float, float, float, float, float, float, float, float, float, float, float,
    //float, float> create1, Delegate create2

    public void SetDelegate1(Delegate create)
    {
        Create = (BulletCreate)create;
    }

    public void SetDelegate2(Delegate create)
    {
        CreateSimple = (SimplifiedBulletCreate)Delegate.CreateDelegate(typeof(SimplifiedBulletCreate), 
            create.Target, create.Method);
    }

    /*private static void Worthless(float centerX, float centerY, float directionX, float directionY,
        float speed, float shape = 0, float sizeX = 10, float sizeY = 10, float rotation = 0, float script = -1,
        float sprite = 1, float r = 1, float g = 1, float b = 1, float a = 1)
    {
        GD.Print("Set a worthless method for a delegate, as a dummy value");
    }
    private static void Worthless(Vector2 position, Vector2 direction, float speed,
        float shape = 0, float sizeX = 10, float sizeY = 10, float rotation = 0, float script = -1, float sprite = 1,
        float r = 1, float g = 1, float b = 1, float a = 1)
    {
        GD.Print("Set a simplified worthless method for a delegate, as a dummy value");
    }*/
}