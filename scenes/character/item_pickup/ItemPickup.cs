using Godot;
using System;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.node_selector;
using wortal_v2.addons.physics_character_body;
using wortal_v2.addons.utils;
using wortal_v2.scenes.items;

public partial class ItemPickup : Node
{
    [Export] private float throwForce = 150f;
    [FromOwner] private PhysicsCharacterBody characterBody = null!;

    private Item? item;
    private uint itemCollisionLayer;
   

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pickup"))
        {
            if (item == null)
            {
                var raycastResult = characterBody.RaycastFromCamera(3, CollisionLayer.Item);

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
        itemCollisionLayer = item.CollisionLayer;
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
            CollisionMask = CollisionLayer.World + CollisionLayer.Item,
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
        item.CollisionLayer = itemCollisionLayer;
        item.Freeze = false;

        mesh.Reparent(item);
        var dot = characterBody.Velocity.Dot(characterBody.Forward);
        var forwardMultiplier = dot < .5f
            ? 1f
            : Mathf.Clamp(characterBody.Velocity.Length(), 1.0f, characterBody.Velocity.Length() / 5f + 1f);
        
        item.ApplyCentralImpulse(characterBody.Forward * throwForce * forwardMultiplier);

        item = null;
    }
}