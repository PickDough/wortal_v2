using Godot;
using wortal_v2.addons.physics_character_body;

namespace wortal_v2.scenes.singletons;

public partial class GameState : Node
{
    public static GameState Instance(Node n) => n.GetNode<GameState>("/root/GameState");

    public void CharacterDied(PhysicsCharacterBody characterBody)
    {
        GetTree().ReloadCurrentScene();
    }
}