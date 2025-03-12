using BulletTester.Scripts;
using Godot;

public partial class Main : Node2D
{
    public static BulletField field;
    public static RandomNumberGenerator random = new RandomNumberGenerator();
    bool mouseColliding = true;
    public Rect2 viewportRect;

    public Emitter selectedEmitter = null;
    public Pattern[] allPatterns = {new Circle(), new MultiplePoints(), new Falling(), new Flower(),
            new Line(), new Aimed() };
    public bool paused = false;

    [Signal]
    public delegate void ReselectEmitterEventHandler();
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
            /*myPattern.Update(new Vector2(field.MouseCollider.GetCenter().X, field.MouseCollider.GetCenter().Y), 
                delta);*/
        }

        HandleMouseInputs();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("mouse_left_click"))
        {
            //GD.Print("Left click pressed");
            mouseColliding = false;
            bool foundTarget = false;
            foreach (var emitter in field.Emitters)
            {
                if (selectedEmitter == emitter)
                {
                    foundTarget = true;
                    continue;
                }
                if (GetViewport().GetMousePosition().DistanceTo(emitter.position) <= 22)
                {
                    selectedEmitter = emitter;
                    SetTarget(emitter);
                    return;
                }
            }
            if (!foundTarget) selectedEmitter = null;
        } else if (@event.IsActionReleased("mouse_left_click"))
        {
            //GD.Print("Left click released");
            mouseColliding = true;
        }

        if (@event.IsActionPressed("ctrl_left_click"))
        {
            //GD.Print("Ctrl left click pressed");
            mouseColliding = false;
            /*field.MouseCollider = new Aabb(GetViewport().GetMousePosition().X,
                GetViewport().GetMousePosition().Y, 0, 0, 0, 0);*/

            Emitter toAdd = new Emitter(GetViewport().GetMousePosition(), (int)Patterns.Circle);
            field.Emitters.Add(toAdd);
            selectedEmitter = toAdd;
            SetTarget(toAdd);
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
            //field.Colliders.Add(mouseAabb);

            foreach (var emitter in field.Emitters)
            {
                if (GetViewport().GetMousePosition().DistanceTo(emitter.position) <= 20)
                {
                    emitter.queueDelete = true;
                    return;
                }
            }
        }

        if (@event.IsActionPressed("ctrl_right_click"))
        {
            //GD.Print("Ctrl right click released");
            field.Colliders.Clear();

            foreach (var emitter in field.Emitters)
            {
                if (GetViewport().GetMousePosition().DistanceTo(emitter.position) <= 20)
                {
                    emitter.process = !emitter.process;
                    return;
                }
            }
        }

        if (@event.IsActionPressed("pause"))
        {
            //GD.Print("Ctrl right click released");
            paused = !paused;
        }

        if (@event.IsActionPressed("delete"))
        {
            field.ClearScreen();
            selectedEmitter = null;
            field.Emitters.Clear();
        }
    }

    public void HandleMouseInputs()
    {
        if (mouseColliding)
        {
            Aabb mouseAabb = new Aabb(GetViewport().GetMousePosition().X - (10),
                GetViewport().GetMousePosition().Y - (10), 0,
                20, 20, 1);
            field.MouseCollider = mouseAabb;
        }
    }

    public void ReloadPatterns()
    {
    }

    public void SetTarget(Emitter emitter)
    {
        selectedEmitter = emitter;

        selectedEmitter.SetDefaults(viewportRect.Size, field.AddProjectile);

        EmitSignal(SignalName.FinishedPatternChange);
    }

    public void ChangePattern(int patternIndex)
    {
        if (selectedEmitter == null) return;
        selectedEmitter.SetPattern(patternIndex, viewportRect.Size);
        EmitSignal(SignalName.FinishedPatternChange);
    }

    public void Begin()
    {
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
