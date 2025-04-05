using Godot;
using Godot.Collections;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.utils;

namespace wortal_v2.addons.physics_character_body;

public partial class PhysicsCharacterBody : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export(PropertyHint.Range, "0.01,1,0.01")]
    public float MouseSensitivity = 0.5f;
    
    [ExportCategory("RigidBody Interaction")]
    [Export] public float PushForce = 20.0f;
    [Export] public float Mass = 80.0f;

    [ExportCategory("Physics")] 
    [Export] public float Friction { get; set; } = 60f;
    [Export] public float OnFloorFrictionMultiplayer { get; set; } = 1.2f;

    [FromOwner(FromSelf = true)] private Camera3D camera;
    
    public Vector3 Forward => -GlobalTransform.Basis.Z;
    
    public Vector3 Impulse;
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        GD.Print(Impulse);

        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
            Impulse = Impulse.MoveToward(Vector3.Zero, Friction * (float)delta);
        }
        else
        {
            Impulse = Impulse.MoveToward(Vector3.Zero, OnFloorFrictionMultiplayer * Friction * (float)delta);
        }
        
        Velocity += Impulse;
        Impulse.Y = 0;
  
        PushRigidBodies();
        MoveAndSlide();
    }

    private void PushRigidBodies()
    {
        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is RigidBody3D area)
            {
                var pushDir = Velocity.Normalized();
                var velocityDiff = Velocity.Dot(pushDir) - area.LinearVelocity.Dot(pushDir);
                velocityDiff = Mathf.Max(0f, velocityDiff);
                var massRatio = Mathf.Min(1f, Mass / area.Mass);
                pushDir.Y = 0;
                
                area.ApplyImpulse(pushDir * velocityDiff * PushForce * massRatio,
                    collision.GetPosition() - area.GlobalPosition);
            }
        }
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
            ? new RaycastResult((CollisionObject3D)result["collider"], (Vector3)result["normal"], (Vector3)result["position"])
            : null;
    }
}