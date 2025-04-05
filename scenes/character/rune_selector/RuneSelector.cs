using Godot;

namespace wortal_v2.scenes.character.rune_selector;

public partial class RuneSelector : Node
{
    [Export] private PackedScene? Input1;
    [Export] private PackedScene? Input2;
    [Export] private PackedScene? Input3;
    [Export] private PackedScene? Input4;

    public PackedScene CurrentRune { get; private set; } = null!;
    
    public override void _Ready()
    {
        if (Input1 != null) CurrentRune = Input1;
        else if (Input2 != null) CurrentRune = Input2;
        else if (Input3 != null) CurrentRune = Input3;
        else if (Input4 != null) CurrentRune = Input4;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("1") && Input1 != null) CurrentRune = Input1;
        else if (@event.IsActionPressed("2") && Input2 != null) CurrentRune = Input2;
        else if (@event.IsActionPressed("3") && Input3 != null) CurrentRune = Input3;
        else if (@event.IsActionPressed("4") && Input4 != null) CurrentRune = Input4;
    }
}