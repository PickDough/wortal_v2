using System;
using Godot;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.physics_character_body;

namespace wortal_v2.scenes.runes;

public abstract partial class RunePower : Node
{
    abstract public void Invoke();
    
    public virtual bool IsAffectingCharacter(PhysicsCharacterBody characterBody)
    {
        return false;
    }
}

public partial class Rune : Area3D
{
    [FromOwner(FromSelf = true)] public Sprite3D Sprite { get; private set; } = null!;
    [FromOwner(FromSelf = true)] public MeshInstance3D HighlightMesh { get; private set; } = null!;
    [FromOwner(FromSelf = true)] public RunePower Power { get; private set; } = null!;

    [Export] private RuneStateEnum stateEnum = RuneStateEnum.Invalid;

    public RuneState State
    {
        get;
        set
        {
            value.SetRune(this);
            field = value;
        }
    } = new RuneState.Invalid();

    public override void _Ready()
    {
        State = stateEnum switch
        {
            RuneStateEnum.Invalid => new RuneState.Invalid(),
            RuneStateEnum.Overlapping => new RuneState.Overlapping(),
            RuneStateEnum.NotPlaced => new RuneState.NotPlaced(),
            RuneStateEnum.Placed => new RuneState.Placed(),
            RuneStateEnum.AimedAt => new RuneState.AimedAt(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}