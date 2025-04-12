using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;
using wortal_v2.addons.gd_inject.attributes;
using wortal_v2.addons.node_selector;

namespace wortal_v2.addons.gd_inject;

public partial class GdInject : Node
{
    private readonly System.Collections.Generic.Dictionary<Type, Node> singletons = [];
    private readonly HashSet<Node> resolvedTypes = [];

    [Export] private Array<PackedScene> sceneDependencies = [];

    [Export] private Array<CSharpScript> scriptsDependencies = [];

    public override void _EnterTree()
    {
        GetTree().Root.ChildEnteredTree += OnNodeAdded;
    }

    public override void _Ready()
    {
        foreach (var scene in sceneDependencies)
        {
            var s = scene.Instantiate<Node>();
            AddChild(s);
        }

        foreach (var script in scriptsDependencies)
        {
            var instance = (Node)script.New();
            AddChild(instance);
        }
    }

    private void OnNodeAdded(Node node)
    {
        if (resolvedTypes.Contains(node)) return;
        ResolveDependencies(node);
    }

    public T ResolveDependencies<T>(T node)
        where T : Node
    {
        node.ChildEnteredTree += OnNodeAdded;
        resolvedTypes.Add(node);

        node.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .ToList()
            .ForEach(f => ResolveDependency(f, f.FieldType, node, v => f.SetValue(node, v)));

        node.GetType()
            .GetProperties()
            .ToList()
            .ForEach(p => ResolveDependency(p, p.PropertyType, node, v => p.SetValue(node, v)));

        return node;
    }

    private void ResolveDependency(MemberInfo m, Type t, Node n, Action<Node?> callback)
    {
        var attributes = m.GetCustomAttributes(true);
        if (attributes.Contains(new FromSingletonAttribute()))
        {
            callback(ResolveSingleton(t));
        }
        else if (attributes.Contains(new FromOwnerAttribute()))
        {
            var attr = m.GetCustomAttribute<FromOwnerAttribute>()!;
            var finding = ResolveUnderParent(
                n,
                t,
                attr.Name,
                attr.FromSelf
            );
            if (finding != null)
                callback(finding);
        }
    }

    private static Node? ResolveUnderParent(Node c, Type t, string? name, bool fromSelf)
    {
        var parent = fromSelf || c.Owner == null ? c : c.Owner;
        var node = parent.FindUnder(t, name);
        if (node != null)
        {
            return node;
        }

        GD.PrintErr($"Node with type \"{t}\" not found for {c.Name}");
        return null;
    }

    private Node? ResolveSingleton(Type t)
    {
        if (singletons.TryGetValue(t, out var value)) 
            return value;
        
        value = (Activator.CreateInstance(t) as Node)!;
        singletons[t] = value;
        AddChild(value);

        return value;
    }
}