using Godot;

namespace wortal_v2.scenes.props.volume.water;

[Tool]
public partial class WaterVolume : Volume
{
	protected override void OnSizeSet(Vector3 size)
	{
		base.OnSizeSet(size);
		var mesh = GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
		if (mesh is not { Mesh: PlaneMesh plane }) return;
		
		plane.Size = new Vector2(size.X, size.Z);
		mesh.Position = mesh.Position with { Y = size.Y / 2 };
	}
}
