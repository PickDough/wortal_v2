using Godot;
using Godot.Collections;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.utils;

namespace wortal_v2.addons.physics_character_body;

public partial class PhysicsCharacterBody : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export(PropertyHint.Range, "0.01,1,0.01")] public float MouseSensitivity = 0.5f;
    [FromOwner(FromSelf = true)] private Camera3D camera;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
        }

        MoveAndSlide();
    }

    public RaycastResult? RaycastFromCamera(float distance, uint collisionMask, bool areas = false, bool bodies = true)
    {
        var from = camera.GlobalPosition;
        var to = camera.GlobalPosition + -camera.GlobalTransform.Basis.Z * distance;

        var spaceState = GetWorld3D().DirectSpaceState;
        var result = spaceState.IntersectRay(new PhysicsRayQueryParameters3D
        {
            From = from,
            To = to,
            CollisionMask = collisionMask,
            Exclude = [GetRid()],
            CollideWithAreas = areas,
            CollideWithBodies = bodies,
        });

        return result.Count > 0 
            ? new RaycastResult((GodotObject)result["collider"], (Vector3)result["normal"], (Vector3)result["position"]) 
            : null;
    }
}