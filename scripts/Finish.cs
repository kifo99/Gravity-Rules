using System;
using System.Collections.Generic;
using Godot;

public partial class Finish : Area2D
{
    [Export]
    public string[] RequiredTools { get; set; } = [];

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            GD.Print("Finished game!");
            player.EvaluateCollectedTools(RequiredTools);
        }
    }
}
