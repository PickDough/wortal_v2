using Godot;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.node_selector;
using wortal_v2.addons.physics_character_body;

namespace wortal_v2.scenes.runes.ice.body_states;

public partial class IceCharacterBody(PhysicsCharacterBody characterBody) : Node
{
    [Export] private int timesToFree = 4;
    private Camera3D camera = null!;
    private float originalSpeed;
    private Vector3 originalPosition;
    
    public override void _Ready()
    {
        originalSpeed = characterBody.Speed;
        characterBody.Speed = 0;
        camera = characterBody.FindUnder<Camera3D>()!;
        originalPosition = camera.Position;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_left") && !@event.IsActionPressed("ui_right") &&
            !@event.IsActionPressed("ui_up") && !@event.IsActionPressed("ui_down"))
            return;
        GD.Print("Freeing character body");
        timesToFree--;
        if (timesToFree <= 0)
        {
            QueueFree();
            return;
        }
        var dir = GetCameraDirection(@event);
        var tween = GetTree().CreateTween().BindNode(camera);
        tween.TweenProperty(camera, "position", originalPosition + dir*0.5f, 0.1f);
        tween.TweenProperty(camera, "position", originalPosition, 0.1f);

    }

    public override void _ExitTree()
    {
        characterBody.Speed = originalSpeed;
    }
    
    private Vector3 GetCameraDirection(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_left"))
        {
            return new Vector3(-1, 0, 0);
        }
        if (@event.IsActionPressed("ui_right"))
        {
            return new Vector3(1, 0, 0);
        }
        if (@event.IsActionPressed("ui_up"))
        {
            return new Vector3(0, 0, -1);
        }
        if (@event.IsActionPressed("ui_down"))
        {
            return new Vector3(0, 0, 1);
        }
        return Vector3.Zero;
    }
}