using System;
using Game.Physics;
using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public CardType CurrentCardType;

    private PlayerInventory _inventory;
    private GravityManager _gravityManager;
    private AnimatedSprite2D _animatedSprite2D;
    private bool _wasOnFloorLastFrame = false;
    private float _friction;
    private float _restitution;
    private Godot.Vector2 _velocity = Godot.Vector2.Zero;
    private float _maxFallVelocity = 0f;
    private bool _isDying = false;

    private void _onAnimationFinished()
    {
        GD.Print($"Animation is {_animatedSprite2D.Animation}");
        if (_animatedSprite2D.Animation == "death")
        {
            SetPhysicsProcess(false);
            SetProcess(false);
            _inventory.ClearInventoryOnDeath();
            QueueFree();
            GetTree().ReloadCurrentScene();
        }
    }

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _gravityManager = GetTree().Root.GetNode<GravityManager>("Game");
        _inventory = GetNode<PlayerInventory>("Inventory");
        ApplyCardType(CurrentCardType);
    }

    public void Die()
    {
        if (_isDying)
            return;

        _isDying = true;
        GD.Print("Player died");
        _velocity.X = 0f;
        _velocity.Y = 0f;
        if (_animatedSprite2D != null)
        {
            _animatedSprite2D.Play("death");
            _animatedSprite2D.AnimationFinished += _onAnimationFinished;
        }
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

    public void TryBreakTile(Node2D body, float impactVelocity)
    {
        float force = CurrentCardType.Mass * Math.Abs(impactVelocity);

        float breakForceThreshold = 500f;

        GD.Print($"Trying to break tile with force {force}");

        if (force >= breakForceThreshold)
        {
            GD.Print("Breaking Tile!");

            if (body is TileMapLayer tileMapLayer)
            {
                tileMapLayer.CollisionEnabled = false;
            }

            body.QueueFree();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_isDying)
        {
            Velocity = _velocity;
            MoveAndSlide();
            return;
        }

        float deltaTime = (float)delta;

        _velocity = Velocity;
        ApplyHorizontalInput(ref _velocity, deltaTime);
        ApplyJumpForce(ref _velocity);
        ApplyGravity(ref _velocity, deltaTime);

        if (Math.Abs(_velocity.X) > 0.1f)
        {
            _animatedSprite2D.FlipH = _velocity.X < 0;
            _animatedSprite2D.Play("run");
        }
        else
        {
            _animatedSprite2D.Play("idle");
        }

        if (!IsOnFloor())
        {
            if (_velocity.Y > 0)
                _animatedSprite2D.Play("fall");
            else
                _animatedSprite2D.Play("jump");

            if (_velocity.Y > _maxFallVelocity)
                _maxFallVelocity = _velocity.Y;
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
        if (Input.IsActionJustPressed("right"))
        {
            velocity.X += CurrentCardType.MoveForce / CurrentCardType.Mass;
        }

        if (Input.IsActionJustPressed("left"))
        {
            velocity.X -= CurrentCardType.MoveForce / CurrentCardType.Mass;
        }

        if (velocity.X != 0f)
        {
            float frictionDirection = -Math.Sign(velocity.X);
            float frictionAmount = CurrentCardType.Friction * 500f * deltaTime;

            if (Math.Abs(velocity.X) < 0.1f)
                velocity.X = 0f;
            else
                velocity.X += frictionAmount * frictionDirection;
        }

        velocity.X = Math.Clamp(velocity.X, -CurrentCardType.MaxSpeed, CurrentCardType.MaxSpeed);
    }

    void ApplyJumpForce(ref Godot.Vector2 velocity)
    {
        if (IsOnFloor() && Input.IsActionJustPressed("up"))
        {
            velocity.Y = CurrentCardType.JumpForce / CurrentCardType.Mass;
            _animatedSprite2D.Play("jump");
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
            float minBounceThreshold = _gravityManager.GetGravity() * deltaTime * 2f;

            float bounce = impactVelocity * CurrentCardType.Restitution * 0.8f;
            bounce = MathF.Min(bounce, 800f);
            if (bounce < minBounceThreshold)
            {
                velocity.Y = 0f;
            }
            else
            {
                GD.Print(
                    $"Impact velocity: {impactVelocity} bounce: {bounce} minimal bounce: {minBounceThreshold}"
                );
                velocity.Y = -bounce;
            }
        }
    }
}
