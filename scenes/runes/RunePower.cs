using Godot;
using wortal_v2.addons.physics_character_body;

namespace wortal_v2.scenes.runes;

[GlobalClass]
public abstract partial class RunePower : Node
{
    protected Rune Rune;
    public abstract void Invoke();

    public virtual bool IsAffectingCharacter(PhysicsCharacterBody characterBody)
    {
        return false;
    }
}