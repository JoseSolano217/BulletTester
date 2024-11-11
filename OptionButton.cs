using Godot;
using System;

public partial class OptionButton : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	bool grow = false;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        Control node = (Control)GetNode("ScrollContainer");
        if (grow)
		{
			node.Visible = true;
            if (Size.Y < 300)
            {
                SetSize(Size + new Vector2(0, 6));
            }
		} else
        {
            node.Visible = false;
            if (Size.Y > 25)
            {
                SetSize(Size - new Vector2(0, 6));
            }
        }
	}

	public void MiracleMallet()
	{
		grow = !grow;
	}
}
