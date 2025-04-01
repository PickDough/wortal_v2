using Godot;
using System;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.node_selector;
using wortal_v2.addons.physics_character_body;
using wortal_v2.addons.utils;
using wortal_v2.scenes.items;

public partial class ItemPickup : Node
{
    private const uint CollisionLayer = 16;

    [FromOwner] private PhysicsCharacterBody characterBody = new();

    private Item? item;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pickup"))
        {
            if (item == null)
            {
                var raycastResult = characterBody.RaycastFromCamera(3, CollisionLayer);

                if (raycastResult != null)
                {
                    PickupItem(raycastResult);
                }
            }
            else
            {
                TryDropItem();
            }
        }
    }

    private void PickupItem(RaycastResult raycastResult)
    {
        item = (raycastResult.Collider as Item)!;
        item.Freeze = true;
        item.CollisionLayer = 0;

        var mesh = item.MeshInstance;
        mesh.Reparent(GetParent());

        var tween = CreateTween();
        tween.TweenProperty(mesh, "position", Vector3.Zero, 0.3f);
    }

    private void TryDropItem()
    {
        var spaceState = GetParent<Node3D>().GetWorld3D().GetDirectSpaceState();
        var query = new PhysicsShapeQueryParameters3D()
        {
            CollisionMask = 1 + CollisionLayer,
            Transform = item!.MeshInstance.GlobalTransform,
            Shape = item.CollisionShape!.Shape,
            CollideWithBodies = true,
        };
        var result = spaceState.IntersectShape(query, 1);
        if (result.Count > 0)
        {
            GD.PrintErr($"Can't drop item, colliding with {result[0]["collider"].As<Node3D>().Name}");
            return;
        }

        var mesh = item.MeshInstance;
        item.GlobalPosition = mesh.GlobalPosition;
        item.GlobalRotation = mesh.GlobalRotation;
        item.CollisionLayer = CollisionLayer;
        item.Freeze = false;

        mesh.Reparent(item);
        var dot = characterBody.Velocity.Dot(characterBody.Forward);
        var forwardMultiplier = dot < .5f
            ? 1f
            : Mathf.Clamp(characterBody.Velocity.Length(), 1.0f, characterBody.Velocity.Length() / 5f + 1f);

        item.ApplyCentralImpulse(characterBody.Forward * 200f * forwardMultiplier);

        item = null;
    }
}