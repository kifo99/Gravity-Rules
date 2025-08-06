using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
  [Export]
  public float Mass { get; set; } = 1f;
  [Export]
  public float Acceleration { get; set; } = 800f;
  [Export]
  public float MaxSpeed { get; set; } = 400f;
  [Export]
  public float Friction { get; set; } = 20f;
  [Export]
  public float JumpForce { get; set; } = -500f;
  [Export]
  public float Gravity { get; set; } = 1200f;


  private bool wasOnFloorLastFrame = false;
  private Godot.Vector2 velocity = Godot.Vector2.Zero;


  public override void _PhysicsProcess(double delta)
  {

    float deltaTime = (float)delta;

    velocity = Velocity;
    ApplyHorizontalInput(ref velocity, deltaTime);
    // ApplyInertia(ref velocity, deltaTime);
    ApplyJumpForce(ref velocity);
    ApplyGravity(ref velocity, deltaTime);

    Velocity = velocity;
    MoveAndSlide();
    ApplyBounceReaction(ref velocity);
    Velocity = velocity;
    wasOnFloorLastFrame = IsOnFloor();
  }

  void ApplyHorizontalInput(ref Godot.Vector2 velocity, float deltaTime)
  {
    float input = 0f;
    if (Input.IsActionPressed("right")) input += 1f;
    if (Input.IsActionPressed("left")) input -= 1f;


    if (input != 0f)
    {

      velocity.X += input * Acceleration * deltaTime;
    }
    else
    {
      float frictionDirection = -Math.Sign(velocity.X);

      velocity.X += frictionDirection * Friction * deltaTime;

      if (Math.Abs(velocity.X) < 2f)
      {
        velocity.X = 0f;
      }
    }

    velocity.X = Math.Clamp(velocity.X, -MaxSpeed, MaxSpeed);
  }

  void ApplyJumpForce(ref Godot.Vector2 velocity)
  {
    if (IsOnFloor() && Input.IsActionPressed("up"))
    {
      GD.Print($"Jump Impulse: {-JumpForce / Mass}");
      velocity.Y = JumpForce / Mass;
    }
  }

  void ApplyGravity(ref Godot.Vector2 velocity, float deltaTime)
  {
    velocity.Y += Gravity * deltaTime;
  }

  void ApplyBounceReaction(ref Godot.Vector2 velocity)
  {

    if (!wasOnFloorLastFrame && IsOnFloor() && velocity.Y >= 0f)
    {

      float restitution = 0.6f / MathF.Max(Mass, 0.5f);
      restitution = Math.Clamp(restitution, 0f, 1f);

      float impactVelocity = velocity.Y;


      float bounce = MathF.Sqrt(impactVelocity * restitution * 100f);


      if (bounce < 10f)
      {
        velocity.Y = 0f;
      }
      else
      {
        GD.Print($"Impact velocity: {impactVelocity} bounce: {bounce}");
        velocity.Y = -bounce;
      }

      
    }
  }
}
