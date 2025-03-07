using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
    public Node modNode;
    public static BulletField field;
    RandomNumberGenerator random = new RandomNumberGenerator();
    bool mouseColliding = true;
    public Rect2 viewportRect;

    public List<Pattern> allPatterns = new List<Pattern>();
    public int patternIndex = 0;
    public int patternChangeTimer = 0;
    public Pattern myPattern;
    public bool paused = false;

    [Signal]
    public delegate void FinishedPatternChangeEventHandler();
    [Signal]
    public delegate void PatternListReloadEventHandler();

    public delegate void SimplerAddBullet(Vector2 position, Vector2 direction, float speed, float script = -1, 
        float sprite = 1, float r = 1, float g = 1, float b = 1, float a = 1);
    public SimplerAddBullet SimplerCreateProjectile = ProjectileAddAPI;

    public override void _Ready()
    {
        viewportRect = GetViewportRect();
        random.Randomize();

        field = (BulletField)GetNode("BulletField");

        ReloadPatterns();
    }

    public override void _Process(double delta)
    {
        viewportRect = GetViewportRect();
        if (!paused)
        {
            myPattern.Update(new Vector2(field.MouseCollider.GetCenter().X, field.MouseCollider.GetCenter().Y), 
                delta);
        }

        HandleMouseInputs();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("mouse_left_click"))
        {
            //GD.Print("Left click pressed");
            mouseColliding = false;
        } else if (@event.IsActionReleased("mouse_left_click"))
        {
            //GD.Print("Left click released");
            mouseColliding = true;
        }

        if (@event.IsActionPressed("ctrl_left_click"))
        {
            //GD.Print("Ctrl left click pressed");
            mouseColliding = false;
            field.MouseCollider = new Aabb(GetViewport().GetMousePosition().X,
                GetViewport().GetMousePosition().Y, 0, 0, 0, 0);
        }
        else if (@event.IsActionReleased("ctrl_left_click"))
        {
            //GD.Print("Ctrl left click released");
            mouseColliding = true;
        }

        if (@event.IsActionPressed("mouse_right_click"))
        {
            //GD.Print("Right click pressed");
            Aabb mouseAabb = new Aabb(GetViewport().GetMousePosition().X - 25,
                GetViewport().GetMousePosition().Y - 25, 0,
                50, 50, 1);
            field.Colliders.Add(mouseAabb);
        }

        if (@event.IsActionPressed("ctrl_right_click"))
        {
            //GD.Print("Ctrl right click released");
            field.Colliders.Clear();
        }

        if (@event.IsActionPressed("pause"))
        {
            //GD.Print("Ctrl right click released");
            paused = !paused;
        }

        if (@event.IsActionPressed("delete"))
        {
            field.ClearScreen();
        }
    }

    public void HandleMouseInputs()
    {
        if (mouseColliding)
        {
            Aabb mouseAabb = new Aabb(GetViewport().GetMousePosition().X - (25),
                GetViewport().GetMousePosition().Y - (25), 0,
                50, 50, 1);
            field.MouseCollider = mouseAabb;
        }
    }

    public void ReloadPatterns()
    {
        allPatterns.Clear();
        allPatterns = new List<Pattern> { 
            new Circle(), 
            new MultiplePoints(), 
            new Flower() , 
            new Line(),
            new Falling()
        };

        patternIndex = 0;
        myPattern = allPatterns[patternIndex];

        myPattern.SetSize(viewportRect.Size.X, viewportRect.Size.Y);
        /*myPattern.GetType().GetMethod("SetSize").Invoke(myPattern,
            new object[] { viewportRect.Size.X, viewportRect.Size.Y });

        Delegate foo = new Action<Vector2, Vector2, float, float, float, float, float, float,
            float, float, float, float, float>((a, b, c, d, e, f, g, h, i, j, k, l, m) => 
            ProjectileAddAPI(a, b, c, d, e, f, g, h, i, j, k, l, m));*/


        myPattern.CreateSimple = field.AddProjectile;
        /*myPattern.GetType().GetMethod("SetDelegate2")
            .Invoke(myPattern, new object[] { foo });*/
        /*(Action<Vector2, Vector2, float, float, float, float, float, float, 
            float, float, float, float, float>)((a, b, c, d, e, f, g, h, i, j, k, l, m) => 
            field.AddProjectile(a, b, c, d, e, f, g, h, i, j, k, l, m))*/

        myPattern.SetDefaults();
        //myPattern.GetType().GetMethod("SetDefaults").Invoke(myPattern, null);
    }

    public void DoNothing()
    {

    }

    public void SetPattern(int newIndex)
    {
        patternIndex = newIndex;
        myPattern = allPatterns[patternIndex];

        myPattern.SetSize(viewportRect.Size.X, viewportRect.Size.Y);

        //myPattern.Connect("BulletCreate", new Callable(field, "AddProjectile"));
        //myPattern.Connect("SimplifiedBulletCreate", new Callable(field, "AddProjectile"));
        //myPattern.Create = field.AddProjectile;
        myPattern.CreateSimple = field.AddProjectile;

        myPattern.SetDefaults();
        //myPattern.GetType().GetMethod("SetDefaults").Invoke(myPattern, null);
        EmitSignal(SignalName.FinishedPatternChange);
    }

    public void Begin()
    {
        SetPattern(0);
        field.ClearScreen();
        Control uiControl = (Control)GetNode("UI");
        uiControl.Visible = true;
    }

    public static void ProjectileAddAPI(Vector2 position, Vector2 direction, float speed, float script = -1, 
        float sprite = 1, float r = 1, float g = 1, float b = 1, float a = 1)
    {
        field.AddProjectile(position, direction, speed, script, sprite, r, g, b, a);
    }

    public int BulletCountAPI()
    {
        return field.ActiveBullets.Count;
    }
}
