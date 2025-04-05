using Godot;
using System;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.physics_character_body;
using wortal_v2.scenes.runes;

public partial class WindRunePower : RunePower
{
    [Export] private float rigidBodyForce = 500f;
    [Export] private Vector3 characterForce = new Vector3(30f, 10f, 30f);

    [FromOwner] private GpuParticles3D windParticles = null!;
    [FromOwner(Name = "Area3D")] private Area3D area = null!;

    public override void Invoke()
    {
        windParticles.Restart();
        foreach (var body in area.GetOverlappingBodies())
        {
            switch (body)
            {
                case RigidBody3D rigidBody:
                    PushRigidBody(rigidBody);
                    break;
                case PhysicsCharacterBody characterBody:
                    PushCharacterBody(characterBody);
                    break;
                default:
                    GD.PrintErr($"Wind rune power cannot push {body}");
                    break;
            }
        }
    }

    private void PushRigidBody(RigidBody3D body)
    {
        var forward = -area.GlobalTransform.Basis.Z;
        body.ApplyCentralImpulse(forward * rigidBodyForce);
    }

    private void PushCharacterBody(PhysicsCharacterBody body)
    {
        var forward = -area.GlobalTransform.Basis.Z;
        body.Impulse += forward * characterForce;
    }
}