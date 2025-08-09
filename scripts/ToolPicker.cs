using Godot;
using Game.Items;
using System;

public partial class ToolPicker : Area2D
{
  [Export] Tool Tool { get; set; }
  public void OnBodyEntered(Node2D body) {
    if (body is Player player)
    {
      player.AddToInventory(Tool.ToolName, Tool.Amount);
      GD.Print($"Added {Tool.Amount} of {Tool.ToolName} to inventory.");
      QueueFree();
    }
  }
}
