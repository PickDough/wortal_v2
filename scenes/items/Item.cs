using Godot;
using wortal_v2.addons.gd_inject.attributes;

namespace wortal_v2.scenes.items;

public partial class Item : RigidBody3D
{
	[FromOwner(FromSelf = true)] public MeshInstance3D MeshInstance { get; set; } = null!;
	[FromOwner(FromSelf = true)] public CollisionShape3D CollisionShape { get; set; } = null!;
}
