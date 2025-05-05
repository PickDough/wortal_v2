using Godot;

namespace wortal_v2.scenes.runes.ice.body_states;

public partial class IceWaterVolume(IcePlane icePlane, IceProjectile iceProjectile): Node3D
{
    public override void _Ready()
    {
        AddChild(icePlane);
        icePlane.GlobalPosition = iceProjectile.GlobalPosition - Vector3.Up * 0.75f;
    }
}