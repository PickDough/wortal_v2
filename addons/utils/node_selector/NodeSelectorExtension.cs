using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace wortal_v2.addons.node_selector;

public static class NodeSelectorExtension
{
    public static T? FinUnderRoot<T>(this Node node)
        where T : Node
    {
        return node.GetTree().Root.FindUnder<T>();
    }

    public static IEnumerable<T> FindUnderMany<T>(
        this Node node,
        bool recursive = false
    )
        where T : Node
    {
        foreach (var c in node.GetChildren())
        {
            if (recursive)
                foreach (var sub in c.FindUnderMany<T>(recursive))
                    yield return sub;

            if (c is T ct) yield return ct;
        }
    }

    public static T? FindUnder<T>(this Node? node)
        where T : Node
    {
        return node switch
        {
            null => null,
            T currentNode => currentNode,
            _ => node.GetChildren().Select(c => c.FindUnder<T>()).OfType<T>().FirstOrDefault()
        };
    }

    public static T? FindUnder<T>(this Node? node, string name) where T : Node
    {
        return FindUnderWithName(node, typeof(T), name) as T;
    }

    public static Node? FindUnder(this Node node, Type type, string? name)
    {
        return name == null ? node.FindUnder(type) : node.FindUnderWithName(type, name);
    }

    private static Node? FindUnder(this Node? node, Type type)
    {
        if (node == null) return null;

        return node.GetType().IsAssignableTo(type)
            ? node
            : node.GetChildren().Select(c => c.FindUnder(type)).OfType<Node>().FirstOrDefault();
    }

    private static Node? FindUnderWithName(this Node? node, Type type, string name)
    {
        if (node == null) return null;

        if (node.GetType().IsAssignableTo(type) && node.Name == name) return node;

        return node.GetChildren().Select(c => c.FindUnderWithName(type, name)).OfType<Node>().FirstOrDefault();
    }
}