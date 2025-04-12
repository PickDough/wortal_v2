using Godot;
using System;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.scenes.keys_locks;

public partial class Door : ActivationTarget
{
    [FromOwner(FromSelf = true)] private AnimationPlayer animationPlayer = null!;
    
    public override void Activate(ActivationTrigger trigger)
    {
        animationPlayer.Play("open_animation");
    }
    
    public override void Deactivate(ActivationTrigger trigger)
    {
        animationPlayer.Play("open_animation", -1, -1, true);
    }
}
