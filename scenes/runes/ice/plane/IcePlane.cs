using Godot;
using System;
using wortal_v2.addons.gd_inject.attributes;

public partial class IcePlane : Node3D
{
    [FromOwner(FromSelf = true)] private AnimationPlayer _animationPlayer = null!;

    public override void _Ready()
    {
        CallDeferred(nameof(PlayAnimation));

    }
    
    private void PlayAnimation()
    {
        _animationPlayer.GetAnimationList();
        foreach (var a in _animationPlayer.GetAnimationList())   
        {
            GD.Print($"Ice Plane animations {a}");
        }
        _animationPlayer.Play("pop_up");
    }
}
