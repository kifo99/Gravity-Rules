using Godot;
using System;
using Game.Physics;
using System.Numerics;

public partial class Player : CharacterBody2D
{
  [Export] public CardType CurrentCardType;
  [Export] public float Gravity { get; set; } = 800f;



  private PlayerInventory _inventory;
  private GravityManager _gravityManager;
  private bool _wasOnFloorLastFrame = false;
  private float _friction;
  private float _restitution;
  private Godot.Vector2 _velocity = Godot.Vector2.Zero;
  private float _maxFallVelocity = 0f;

  public override void _Ready()
  {
    _gravityManager = GetTree().Root.GetNode<GravityManager>("Game");
    _inventory = GetNode<PlayerInventory>("Inventory");
    ApplyCardType(CurrentCardType);
  }

  public void ApplyCardType(CardType cardType)
  {
    CurrentCardType = cardType;

    _friction = cardType.Friction;
    _restitution = cardType.Restitution;
  }

  public void AddToInventory(string toolName, int amount)
  {
    _inventory.AddTools(toolName, amount);
  }

  public override void _PhysicsProcess(double delta)
  {

    float deltaTime = (float)delta;

    _velocity = Velocity;
    ApplyHorizontalInput(ref _velocity, deltaTime);
    ApplyJumpForce(ref _velocity);
    ApplyGravity(ref _velocity, deltaTime);

    if (!IsOnFloor())
    {
      if (_velocity.Y > _maxFallVelocity) _maxFallVelocity = _velocity.Y;
    }

    Velocity = _velocity;
    MoveAndSlide();

    if (!_wasOnFloorLastFrame && IsOnFloor())
    {
      float impactVelocity = _maxFallVelocity;
      ApplyBounceReaction(ref _velocity, impactVelocity, deltaTime);
      Velocity = _velocity;
      _maxFallVelocity = 0f;
    }
    _wasOnFloorLastFrame = IsOnFloor();
  }

  void ApplyHorizontalInput(ref Godot.Vector2 velocity, float deltaTime)
  {
    float input = 0f;
    if (Input.IsActionPressed("right")) input += 1f;
    if (Input.IsActionPressed("left")) input -= 1f;

    float acceleration = input * CurrentCardType.MoveForce / CurrentCardType.Mass;

    if (input != 0f)
    {

      velocity.X += acceleration * deltaTime;
    }
    else
    {
      float frictionDirection = -Math.Sign(velocity.X);
      float frictionAmount = CurrentCardType.Friction * 1000f;


      velocity.X += frictionDirection * frictionAmount * deltaTime;



      if (Math.Abs(velocity.X) < 1f)
      {
        velocity.X = 0f;
      }
    }

    velocity.X = Math.Clamp(velocity.X, -CurrentCardType.MaxSpeed, CurrentCardType.MaxSpeed);
  }

  void ApplyJumpForce(ref Godot.Vector2 velocity)
  {
    if (IsOnFloor() && Input.IsActionPressed("up"))
    {
      GD.Print($"Jump Impulse: {-CurrentCardType.JumpForce / CurrentCardType.Mass}");
      velocity.Y = CurrentCardType.JumpForce / CurrentCardType.Mass;
    }
  }

  void ApplyGravity(ref Godot.Vector2 velocity, float deltaTime)
  {
    if (_gravityManager != null)
    {
      velocity.Y += _gravityManager.GetGravity() * deltaTime;
    }
  }

  void ApplyBounceReaction(ref Godot.Vector2 velocity, float impactVelocity, float deltaTime)
  {

    if (!_wasOnFloorLastFrame && IsOnFloor() && velocity.Y >= 0f)
    {

      float minBounceThreshold = Gravity * deltaTime * 2f;

      float bounce = impactVelocity * CurrentCardType.Restitution * 0.8f;
      bounce = MathF.Min(bounce, 800f);
      if (bounce < minBounceThreshold)
      {
        velocity.Y = 0f;
      }
      else
      {
        GD.Print($"Impact velocity: {impactVelocity} bounce: {bounce} minimal bounce: {minBounceThreshold}");
        velocity.Y = -bounce;
      }


    }
  }
}
