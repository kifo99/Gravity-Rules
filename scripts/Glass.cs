using System;
using Godot;

public partial class Glass : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            GD.Print("Destroyed glass");
        }
    }
}
