using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Pattern
{
    public RandomNumberGenerator random = new RandomNumberGenerator();
    // Gray
    public List<Color>[] palette = { new List<Color>()
    {
        new Color(178, 178, 178)
    },
    // Red
        new List<Color>()
    {
        new Color(255, 66, 66)
    },
    // Pink
        new List<Color>()
    {
        new Color(255, 57, 252)
    },
    // Blue
        new List<Color>()
    {
        new Color(57, 57, 255)
    },
    // Cyan
        new List<Color>()
    {
        new Color(66, 227, 255)
    },
    // Green
        new List<Color>()
    {
        new Color(66, 255, 110)
    },
    // Yellow
        new List<Color>()
    {
        new Color(255, 252, 35)
    },
    // Easy (Blue/Cyan)
        new List<Color>() {
        new Color(57, 57, 255), new Color(66, 227, 255)
    // Normal (Blue/Cyan/Green)
    }, new List<Color>() {
        new Color(57, 57, 255), new Color(66, 227, 255), new Color(66, 255, 110)
    // Hard (Green/Yellow/Red)
    }, new List<Color>() {
        new Color(66, 255, 110), new Color(255, 252, 35), new Color(255, 66, 66)
    // Anomalous (Pink/Red)
    }, new List<Color>() {
        new Color(255, 66, 66), new Color(255, 57, 252)
    // Monochrome (Black/White)
    }, new List<Color>() {
        new Color(178, 178, 178), new Color(0, 0, 0)
    // All
    }, new List<Color>() {
        new Color(178, 178, 178), new Color(255, 66, 66), new Color(255, 57, 252), new Color(57, 57, 255), 
        new Color(66, 227, 255), new Color(66, 255, 110), new Color(255, 252, 35), new Color(0, 0, 0)
    }};
    [Bindable(true)]
    public int SpriteType { get; set; } = 1;
    [Bindable(true)]
    public float Alpha { get; set; } = 1f;
    [Bindable(true)]
    public int MainPalette { get; set; } = 0;
    [Bindable(true)]
    public int SecondaryPalette { get; set; } = 0;

    public delegate void SimplifiedBulletCreate(Vector2 position, Vector2 direction, float speed, 
        float script = -1, float sprite = 1, float r = 1, float g = 1, float b = 1, float a = 1, float ai1 = 0, 
        float ai2 = 0, float ai3 = 0);
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

    public virtual bool PreUpdate(Vector2? mousePos, Vector2 position)
    {
        return true;
    }

    public virtual void Update(Vector2? mousePos, double delta, Vector2 position)
    {
        if (!PreUpdate(mousePos, position)) return;
        Timer += (float)delta;
        if (Timer >= MaxTimer)
        {
            Cycle++;
            Timer = 0;
        }
        if (Cycle >= MaxCycle) Cycle = 0;
        PostUpdate(mousePos, position);
    }

    public virtual void PostUpdate(Vector2? mousePos, Vector2 position)
    {
    }

    public void SetSize(float width, float height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetDelegate(Delegate create)
    {
        CreateSimple = (SimplifiedBulletCreate)Delegate.CreateDelegate(typeof(SimplifiedBulletCreate), 
            create.Target, create.Method);
    }
}