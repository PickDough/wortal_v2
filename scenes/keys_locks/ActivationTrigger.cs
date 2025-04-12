using System;
using System.Threading.Tasks;
using Godot;

namespace wortal_v2.scenes.keys_locks;

[GlobalClass]
public partial class ActivationTrigger: Node3D
{
    public bool IsTriggered { get; protected set; }
    [Signal] public delegate void ActivateEventHandler(ActivationTrigger triggeredBy);
    [Signal] public delegate void DeactivateEventHandler(ActivationTrigger triggeredBy);
}