using Godot;
using Game.Physics;
using System;

public partial class BallSwitcher : Area2D
{
  [Export] public BallType NewBallType;

  public void OnBodyEntered(Node2D body)
  {
    if (body is Player player)
    {
      player.ApplyBallType(NewBallType);
      GD.Print("Ball type switched to: " + NewBallType.Name);
    }
  } 
}
