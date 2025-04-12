using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace wortal_v2.ui.screens;

public partial class ScreenManager : Node
{
    public static ScreenManager Instance(Node n) => n.GetNode<ScreenManager>("/root/ScreenManager");
    [Export] private Array<PackedScene> PackedScreens { get; set; } = [];
    private List<Control> Screens { get; set; } = [];
    public Control? CurrentScreen { get; private set; }

    public override void _Ready()
    {
        Screens = PackedScreens
            .Select(packedScene => packedScene.Instantiate())
            .Cast<Control>()
            .ToList();
    }

    public void SetScreen<T>()
    {
        var screen = Screens.First(s => s is T);

        ClearScreen();
        GetTree().CurrentScene.AddChild(screen);
        CurrentScreen = screen;
        Input.SetMouseMode(Input.MouseModeEnum.Visible);
    }
    
    public void ClearScreen()
    {
        CurrentScreen?.GetParent()?.RemoveChild(CurrentScreen);
        CurrentScreen = null;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && CurrentScreen == null)
        {
            Input.SetMouseMode(Input.MouseModeEnum.Visible);
            Engine.TimeScale = 0;
        }
        else if (Input.GetMouseMode() == Input.MouseModeEnum.Visible && CurrentScreen == null &&
                 @event is InputEventMouseButton { Pressed: true })
        {
            Input.SetMouseMode(Input.MouseModeEnum.Captured);
            Engine.TimeScale = 1;
        }
    }
}