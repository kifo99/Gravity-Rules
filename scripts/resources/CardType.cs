using System;
using Godot;

namespace Game.Physics
{
    [GlobalClass]
    public partial class CardType : Resource
    {
        [Export]
        public string Name { get; set; } = "Default";

        [Export]
        public float Mass { get; set; } = 1f;

        [Export]
        public float Restitution { get; set; } = 0.5f;

        [Export]
        public float Friction { get; set; } = 0.3f;

        [Export]
        public float MaxSpeed { get; set; } = 400f;

        [Export]
        public float JumpForce { get; set; } = -500f;

        [Export]
        public float MoveForce { get; set; } = 400f;
    }
}
