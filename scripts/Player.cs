using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
  [Export]
  public float Acceleration { get; set; } = 800f;
  [Export]
  public float MaxSpeed { get; set; } = 400f;
  [Export]
  public float Friction { get; set; } = 200f;
  [Export]
  public float JumpForce { get; set; } = -500f;
  [Export]
  public float Gravity { get; set; } = 1200f;

    Godot.Vector2 velocity;


  public override void _PhysicsProcess(double delta)
  {

    float deltaTime = (float)delta;

    velocity = Velocity;
    ApplyHorizontalInput(ref velocity, deltaTime);
    ApplyInertia(ref velocity, deltaTime);
    ApplyJumpForce(ref velocity);
    ApplyGravity(ref velocity,deltaTime);


    Velocity = velocity;
    MoveAndSlide();

  }

  void ApplyHorizontalInput(ref Godot.Vector2 velocity, float deltaTime)
  {
    float direction = 0f;
    if (Input.IsActionPressed("right")) direction += 1f;
    if (Input.IsActionPressed("left")) direction -= 1f;

    
    velocity.X += direction * Acceleration * deltaTime;

    velocity.X = Math.Clamp(velocity.X, -MaxSpeed, MaxSpeed);
  }

  void ApplyInertia(ref Godot.Vector2 velocity, float deltaTime)
  {
    if (!Input.IsActionPressed("right") && !Input.IsActionPressed("left"))
    {
      
      if (Math.Abs(velocity.X) > 0.01f)
      {
        float frictionDirection = -Math.Sign(velocity.X);

        velocity.X += frictionDirection * Friction * deltaTime;

        if (Math.Sign(velocity.X) != frictionDirection)
        {
          velocity.X = 0f;
        }
      }
    }
  }

  void ApplyJumpForce(ref Godot.Vector2 velocity)
  {
    if (IsOnFloor() && Input.IsActionPressed("up"))
    {
      velocity.Y = JumpForce;
    }
  }

  void ApplyGravity(ref Godot.Vector2 velocity, float deltaTime)
  {
    velocity.Y += Gravity * deltaTime;
  }

  void ApplyBounceReaction(ref Godot.Vector2 velocity, float bounceForce)
  {
    velocity.Y = -bounceForce;
  }
}
