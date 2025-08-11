using System;
using Godot;

public partial class Detect : Area2D
{
    private Player _player;

    public override void _Ready()
    {
        BodyEntered += OnBOdyEntered;
        _player = GetParent<Player>();
    }

    private async void OnBOdyEntered(Node2D body)
    {
        GD.Print($"Called and detected: {body.Name}");
        if (body.Name == "BreakableGround")
        {
            GD.Print("Ground broken");

            float impactVelocity = _player.Velocity.Y;

            _player.TryBreakTile(body, impactVelocity);
        }
    }
}
