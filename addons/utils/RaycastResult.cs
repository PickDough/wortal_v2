using Godot;

namespace wortal_v2.addons.utils;

public record RaycastResult(GodotObject Collider, Vector3 Normal, Vector3 Point);