using Godot;
using wortal_v2.addons.gd_inject.attributes;

namespace wortal_v2.scenes.runes;

public abstract partial class RunePower : Node
{
	abstract public void Invoke();
}

public partial class Rune : Area3D
{
	[FromOwner(FromSelf = true)] public Sprite3D Sprite { get; private set; } = null!;
	[FromOwner(FromSelf = true)] public MeshInstance3D HighlightMesh { get; private set; } = null!;
	[FromOwner(FromSelf = true)] public RunePower Power { get; private set; } = null!;

	public RuneState State
	{
		get;
		set
		{
			value.SetRune(this);
			field = value;
		}
	} = new RuneState.Invalid();
}