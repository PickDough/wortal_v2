using System.Threading.Tasks;
using Godot;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.scenes.keys_locks;

namespace wortal_v2.scenes.props.super_button;

public partial class SuperButton : ActivationTrigger
{
    [FromOwner(FromSelf = true)] private AnimationPlayer animationPlayer = null!;
    [FromOwner(FromSelf = true)] private Area3D area = null!;
    private bool isPushed;

    public override void _Ready()
    {
        area.BodyEntered += async _ =>
        {
            if (area.GetOverlappingBodies().Count < 2)
                await Push();
        };
        area.BodyExited += async _ =>
        {
            if (area.GetOverlappingBodies().Count == 0)
                await Lift();
        };
    }

    private async Task Push()
    {
        animationPlayer.Play("push_down");
        isPushed = true;
        await ToSignal(animationPlayer, AnimationMixer.SignalName.AnimationFinished);
        if (isPushed && !IsTriggered)
        {
            EmitSignal("Activate", this);
            IsTriggered = true;
        }
    }

    private Task Lift()
    {
        animationPlayer.Play("push_down", -1, -1, true);
        isPushed = false;
        if (!isPushed && IsTriggered)
        {
            EmitSignal("Deactivate", this);
            IsTriggered = false;
        }

        return Task.CompletedTask;
    }
}