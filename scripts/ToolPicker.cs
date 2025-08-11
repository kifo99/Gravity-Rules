using System;
using Game.Items;
using Godot;

public partial class ToolPicker : Area2D
{
    [Export]
    Tool Tool { get; set; }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print($"Body name {body.Name}");
        if (body is Player player)
        {
            player.AddToInventory(Tool.ToolName, Tool.Amount);
            GD.Print($"Added {Tool.Amount} of {Tool.ToolName} to inventory.");
            QueueFree();
        }
    }
}
