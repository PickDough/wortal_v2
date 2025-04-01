using System;
using Godot;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.physics_character_body;

namespace wortal_v2.scenes.character.rune_placer;

public partial class RunePlacer : Node
{
    public const uint CollisionLayer = 4;
    
    [Export] private PackedScene? sceneToPlace;
    [FromOwner] private PhysicsCharacterBody characterBody = new();
    
    private Node3D? rune;

    public override void _Process(double delta)
    {
        if (rune == null)
            return;
        
        var raycastResult = characterBody.RaycastFromCamera(10, CollisionLayer);
        if (raycastResult == null)
            return;
        
        rune.GlobalPosition = RuneSurfaceResolver.ResolveSurface(raycastResult);
        if (raycastResult.Normal != Vector3.Up)
            rune.LookAt(rune.GlobalPosition + raycastResult.Normal);
        else
            rune.RotationDegrees = new Vector3(90f, 0f, 0f);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Right })
        {
            if (rune == null && sceneToPlace != null)
            {
                rune = sceneToPlace.Instantiate<Node3D>();
                AddChild(rune);
            }
        }
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsReleased() && rune != null)
        {
            rune.QueueFree();
            rune = null;
        }
    }
}