using System;
using Godot;

public partial class Killzone : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Die();
        }
    }
}
