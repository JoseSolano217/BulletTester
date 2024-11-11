using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

public partial class UI : Control
{
	Main main;
    PackedScene areaScene = (PackedScene)ResourceLoader.Load("res://PropertyArea.tscn");
    PackedScene vector2Control = (PackedScene)ResourceLoader.Load("res://Controls/Vector2Spinbox.tscn");
    string areaNodePath = "Panel/VSplitContainer/PanelContainer/ScrollContainer/VBoxContainer";
    string patternAreaNodePath = "Panel/VSplitContainer/PatternMenu/ScrollContainer/VBoxContainer";
    // Key: Property index, Value: Node index
    Dictionary<int, int> propertyConnection = new Dictionary<int, int>();
    bool show = false;

    [Signal]
    public delegate void PatternChangeEventHandler(int newIndex);

    public override void _Ready()
    {
        main = (Main)GetParent();

        Connect("PatternChange", new Callable(main, "SetPattern"));
        main.Connect("FinishedPatternChange", new Callable(this, "SetPropertyFields"));

        CallDeferred("SetPropertyFields");
        CallDeferred("SetPatternFields");
    }

	public override void _Process(double delta)
    {
		UpdateData(delta);

        Panel panel = (Panel)GetNode("Panel");
        if (!show)
        {
            if (panel.Position.X > -176)
            {
                panel.SetPosition(panel.Position - new Vector2(4, 0));
            }
        } else
        {
            if (panel.Position.X < 0)
            {
                panel.SetPosition(panel.Position + new Vector2(4, 0));
            }
        }
    }

    private void UpdateData(double delta)
    {
        Label fps = (Label)GetNode("FPS");
        Label bullets = (Label)GetNode("Bullets");
        fps.Text = (60 / (60 * delta)) + " FPS";
        bullets.Text = main.BulletCountAPI() + " Bullets";

        foreach (int key in propertyConnection.Keys)
        {
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(main.myPattern)[key];
            Label valueShowcaser = (Label)GetNode(areaNodePath).GetChild(propertyConnection[key]).GetChild(0).GetChild(1);
            Node valueContainer = GetNode(areaNodePath).GetChild(propertyConnection[key]).GetChild(0).GetChild(2);

            Type type = descriptor.GetValue(main.myPattern).GetType();
            Object value = descriptor.GetValue(main.myPattern);
            if (value == null) continue;

            valueShowcaser.Text = value.ToString();
        }
    }

    private void SetPropertyFields()
    {
        propertyConnection.Clear();

        foreach (Node child in GetNode(areaNodePath).GetChildren())
        {
            GetNode(areaNodePath).RemoveChild(child);
            child.QueueFree();
        }

        for (int i = 0; i < TypeDescriptor.GetProperties(main.myPattern).Count; i++)
        {
            AttributeCollection attribute = TypeDescriptor.GetProperties(main.myPattern)[i].Attributes;

            if (attribute[typeof(BindableAttribute)].Equals(BindableAttribute.Yes))
            {
                if (TypeDescriptor.GetProperties(main.myPattern)[i].GetValue(main.myPattern) == null) continue;
                Type attributeType = TypeDescriptor.GetProperties(main.myPattern)[i].GetValue(main.myPattern).GetType();
                if (attributeType == null) continue;
                if (attributeType == typeof(Delegate)) continue;
                Object attributeValue = TypeDescriptor.GetProperties(main.myPattern)[i].GetValue(main.myPattern);
                Label label = new Label();
                label.Text = AddSpacesToSentence(TypeDescriptor.GetProperties(main.myPattern)[i].Name);
                if (TypeDescriptor.GetProperties(main.myPattern)[i].IsReadOnly)
                {
                    label.Text += " (Read only)";
                }

                Node area = areaScene.Instantiate();
                area.GetChild(0).AddChild(label);

                Label valueLabel = new Label();
                valueLabel.Text = "Value: " + 
                    TypeDescriptor.GetProperties(main.myPattern)[i].GetValue(main.myPattern).ToString();
                area.GetChild(0).AddChild(valueLabel);

                if (attributeType == typeof(int))
                {
                    SpinBox spinBox = new SpinBox();
                    spinBox.AllowGreater = true;
                    spinBox.AllowLesser = true;
                    spinBox.Step = 1;
                    spinBox.FocusMode = FocusModeEnum.Click;
                    if (TypeDescriptor.GetProperties(main.myPattern)[i].IsReadOnly)
                    {
                        spinBox.Editable = false;
                    }
                    spinBox.Value = (int)attributeValue;
                    int index = i;
                    spinBox.ValueChanged += (val) => OnValueChange(val, index);
                    //spinBox.Connect("value_changed", new Callable(this, "OnValueChange"), (uint)i);
                    area.GetChild(0).AddChild(spinBox);
                } else if (attributeType == typeof(double) || attributeType == typeof(float) || 
                    attributeType == typeof(Single))
                {
                    SpinBox spinBox = new SpinBox();
                    spinBox.AllowGreater = true;
                    spinBox.AllowLesser = true;
                    spinBox.Step = 0.0001;
                    spinBox.FocusMode = FocusModeEnum.Click;
                    if (TypeDescriptor.GetProperties(main.myPattern)[i].IsReadOnly)
                    {
                        spinBox.Editable = false;
                    }
                    spinBox.Value = (Single)attributeValue;
                    int index = i;
                    spinBox.ValueChanged += (val) => OnValueChange(val, index);
                    //spinBox.Connect("value_changed", new Callable(this, "OnValueChange"), (uint)i);
                    area.GetChild(0).AddChild(spinBox);
                } else if (attributeType == typeof(bool))
                {
                    CheckButton checkButton = new CheckButton();
                    if (TypeDescriptor.GetProperties(main.myPattern)[i].IsReadOnly)
                    {
                        checkButton.Disabled = true;
                    }
                    checkButton.ButtonPressed = (bool)attributeValue;
                    int index = i;
                    checkButton.Toggled += (val) => OnToggled(val, index);
                    //checkButton.Connect("toggled", new Callable(this, "OnToggled"), (uint)i);
                    area.GetChild(0).AddChild(checkButton);
                } else if(attributeType == typeof(Vector2))
                {
                    Vector2Spinbox vector2Spinbox = (Vector2Spinbox)vector2Control.Instantiate();
                    if (!TypeDescriptor.GetProperties(main.myPattern)[i].IsReadOnly)
                    {
                        vector2Spinbox.value = (Vector2)attributeValue;
                        int index = i;
                        vector2Spinbox.ValueChanged += (val) => OnValueChange(val, index);
                        area.GetChild(0).AddChild(vector2Spinbox);
                    }
                } else
                {
                    Label label2 = new Label();
                    label2.Text = "Type not supported";
                    area.GetChild(0).AddChild(label2);
                }
                GetNode(areaNodePath).AddChild(area);
                label.Text = area.GetIndex() + ". " + label.Text;
                propertyConnection[i] = area.GetIndex();

                //GD.Print("Attribute " + i + ": ");
                //GD.Print("Name: " + TypeDescriptor.GetProperties(myPattern)[i].Name);
                //GD.Print("Value: " + TypeDescriptor.GetProperties(myPattern)[i].GetValue(myPattern));
                //TypeDescriptor.GetProperties(myPattern)[i].IsReadOnly;
            }
        }
    }

    public void SetPatternFields()
    {
        foreach (Node child in GetNode(patternAreaNodePath).GetChildren())
        {
            GetNode(patternAreaNodePath).RemoveChild(child);
            child.QueueFree();
        }

        for (int i = 0; i < main.allPatterns.Count; i ++)
       {
            Button patternButton = new Button();
            patternButton.Text = AddSpacesToSentence(main.allPatterns[i].GetType().Name);

            int index = i;
            patternButton.Pressed += () => ChangePattern(index);

            GetNode(patternAreaNodePath).AddChild(patternButton);
        }
    }

    public void ChangePattern(int index)
    {
        EmitSignal(SignalName.PatternChange, index);
    }

    public void OnClickShowButton()
    {
        show = !show;
    }

    public void OnToggled(bool toggledOn, int index)
    {
        TypeDescriptor.GetProperties(main.myPattern)[index].SetValue(main.myPattern, toggledOn);
    }

    public void OnValueChange(double value, int index)
    {
        Type attributeType = TypeDescriptor.GetProperties(main.myPattern)[index].GetValue(main.myPattern).GetType();
        if (attributeType == typeof(int))
        {
            TypeDescriptor.GetProperties(main.myPattern)[index].SetValue(main.myPattern, (int)value);
        } else if (attributeType == typeof(double) || attributeType == typeof(float) || 
            attributeType == typeof(Single))
        {
            TypeDescriptor.GetProperties(main.myPattern)[index].SetValue(main.myPattern, (Single)value);
        } else
        {
            GD.Print("Failed to change value because the value is not supported.");
            GD.Print("Type of the value: " + attributeType);
        }
    }

    public void OnValueChange(Vector2 value, int index)
    {
        Type attributeType = TypeDescriptor.GetProperties(main.myPattern)[index].GetValue(main.myPattern).GetType();
        if (attributeType == typeof(Vector2))
        {
            TypeDescriptor.GetProperties(main.myPattern)[index].SetValue(main.myPattern, (Vector2)value);
        }
        else
        {
            GD.Print("Failed to change value because the value is not supported.");
            GD.Print("Type of the value: " + attributeType);
        }
    }

    string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }
}