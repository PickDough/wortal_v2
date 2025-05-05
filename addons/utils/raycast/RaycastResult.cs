using Godot;

namespace wortal_v2.addons.utils;

public record RaycastResult(CollisionObject3D Collider, Vector3 Normal, Vector3 Point);