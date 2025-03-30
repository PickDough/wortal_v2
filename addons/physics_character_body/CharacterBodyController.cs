using Godot;
using wortal_v2.addons.gd_inject.attributes;

namespace wortal_v2.addons.physics_character_body;

public partial class CharacterBodyController : Node
{
	[FromOwner] private PhysicsCharacterBody characterBody = new ();
	[FromOwner] private Camera3D camera = new ();

	public override void _PhysicsProcess(double delta)
	{
		var velocity = characterBody.Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		var inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		var direction = (characterBody.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * characterBody.Speed;
			velocity.Z = direction.Z * characterBody.Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(characterBody.Velocity.X, 0, characterBody.Speed);
			velocity.Z = Mathf.MoveToward(characterBody.Velocity.Z, 0, characterBody.Speed);
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && characterBody.IsOnFloor())
		{
			velocity.Y = characterBody.JumpVelocity;
		}
		else
		{
			velocity.Y = characterBody.Velocity.Y;
		}
		
		characterBody.Velocity = velocity;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			characterBody.RotationDegrees += new Vector3(0f, -mouseMotion.Relative.X * characterBody.MouseSensitivity, 0f);
			camera.RotationDegrees = new Vector3(Mathf.Clamp(camera.RotationDegrees.X  - mouseMotion.Relative.Y * characterBody.MouseSensitivity, -90f, 90f), 0, 0);
			
		}
	}
}