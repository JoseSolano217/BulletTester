using Godot;

public partial class Vector2Spinbox : Panel
{
	[Export]
	public Vector2 value = Vector2.Zero;
	[Signal]
	public delegate void ValueChangedEventHandler(Vector2 value);

	public void ChangeXValue(float newValue)
	{
		value.X = newValue;
		EmitSignal("ValueChanged", value);
	}

    public void ChangeYValue(float newValue)
    {
        value.Y = newValue;
        EmitSignal("ValueChanged", value);
    }
}
