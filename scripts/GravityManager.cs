using Godot;
using System;

public partial class GravityManager : Node2D
{
  [Export] public float EarthsGravity { get; set; } = 1200f;
  [Export] public float GravityMultiplier { get; set; } = 1f;


  public float GetGravity()
  {
    return EarthsGravity * GravityMultiplier;
  }

}
