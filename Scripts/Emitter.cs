using Godot;
using System;

namespace BulletTester.Scripts
{
    public enum Patterns
    {
        Circle, 
        MultiPoints, 
        Falling, 
        Flower, 
        Line, 
        Aimed
    }

    public class Emitter
    {
        private Pattern[] storedPatterns = { new Circle(), new MultiplePoints(), new Falling(), new Flower(), 
            new Line(), new Aimed()  };
        public Vector2 position;
        public int patternIndex;
        public Color color;
        public bool process = true;
        public bool queueDelete = false;

        public Emitter(Vector2 position, int pattern, Color color)
        {
            this.position = position;
            patternIndex = pattern;
            this.color = color;
        }

        public Emitter(Vector2 position, int pattern)
        {
            this.position = position;
            patternIndex = pattern;
            color = new Color(Main.random.Randf(), Main.random.Randf(), Main.random.Randf());
        }

        public Emitter(Vector2 position)
        {
            this.position = position;
            patternIndex = 0;
            color = new Color(Main.random.Randf(), Main.random.Randf(), Main.random.Randf());
        }

        public void SetDefaults(Vector2 size, Delegate createBullet)
        {
            foreach (var pattern in storedPatterns)
            {
                pattern.SetSize(size.X, size.Y);
                pattern.SetDelegate(createBullet);
                pattern.SetDefaults();
            }
        }

        public void Update(Vector2? mousePos, double delta)
        {
            if (process)
            {
                storedPatterns[patternIndex].Update(mousePos, delta, position);
            }
        }

        public void SetPattern(int newIndex, Vector2 size)
        {
            patternIndex = newIndex;
            if (patternIndex < 0) patternIndex = 0;
            if (patternIndex >= storedPatterns.Length) patternIndex = storedPatterns.Length-1;
            storedPatterns[patternIndex].SetSize(size.X, size.Y);
        }

        public Pattern GetCurrentPattern()
        {
            return storedPatterns[patternIndex];
        }
    }
}
