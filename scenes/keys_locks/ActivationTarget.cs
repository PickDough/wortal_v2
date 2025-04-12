using Godot;

namespace wortal_v2.scenes.keys_locks;

[GlobalClass]
public partial class ActivationTarget : Node3D
{
    public virtual void Activate(ActivationTrigger trigger) {}
    public virtual void Deactivate(ActivationTrigger trigger) {}
}