using System;
using Godot;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.node_selector;
using wortal_v2.addons.physics_character_body;
using wortal_v2.addons.utils;
using wortal_v2.scenes.character.rune_selector;
using wortal_v2.scenes.runes;

namespace wortal_v2.scenes.character.rune_placer;

public partial class RunePlacer : Node
{
    [Export] private float distance = 6f;

    [FromOwner] private PhysicsCharacterBody characterBody = null!;
    [FromOwner] private RuneSelector runeSelector = null!;

    private Rune? rune;

    public override void _Process(double delta)
    {
        if (rune == null)
            return;

        var raycastResult = characterBody.RaycastFromCamera(distance,
            CollisionLayer.World + CollisionLayer.RuneSurface + CollisionLayer.Item);
        if (raycastResult == null || !raycastResult.Collider.GetCollisionLayerValue(CollisionLayer.RuneSurfaceLayer))
        {
            rune.State = new RuneState.Invalid();
            return;
        }

        (rune.GlobalPosition, var normal) = RuneSurfaceResolver.ResolveSurface(raycastResult);
        if (Math.Abs(Math.Abs(normal.Y) - 1f) > .001)
            rune.LookAt(rune.GlobalPosition + normal);
        else
            rune.RotationDegrees = new Vector3(Mathf.Sign(normal.Y)*90f, 0f, 0f);

        rune.Sprite.GlobalPosition = rune.GlobalPosition;
        if (RuneSurfaceResolver.IsOverlapping(raycastResult, rune!))
        {
            rune.State = new RuneState.Overlapping();
            rune.Sprite.GlobalPosition = rune.GlobalPosition + normal * 0.01f;
            return;
        }

        rune.State = new RuneState.NotPlaced();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Right })
        {
            if (rune == null)
            {
                rune = runeSelector.CurrentRune.Instantiate<Rune>();
                AddChild(rune);
            }
        }

        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsReleased() && rune != null)
        {
            PlaceRune();
            rune.QueueFree();
            rune = null;
        }

        if (@event.IsActionPressed("cancel") && rune != null)
        {
            rune.QueueFree();
            rune = null;
        }
    }

    private void PlaceRune()
    {
        var raycastResult = characterBody.RaycastFromCamera(distance,
            CollisionLayer.World + CollisionLayer.RuneSurface + CollisionLayer.Item);
        if (raycastResult == null || RuneSurfaceResolver.IsOverlapping(raycastResult, rune!))
            return;

        var placedRune = rune!.Duplicate() as Rune;
        var mesh = raycastResult.Collider.FindUnder<MeshInstance3D>();
        if (raycastResult.Collider is RigidBody3D)
            mesh!.AddChild(placedRune);
        else
            raycastResult.Collider.AddChild(placedRune);
        
        placedRune!.State = new RuneState.Placed();
        placedRune.GlobalPosition = rune.GlobalPosition;
        placedRune.GlobalRotation = rune.GlobalRotation;
    }
}