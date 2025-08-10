using System;
using Godot;

namespace Game.Items
{
    [GlobalClass]
    public partial class Tool : Resource
    {
        [Export]
        public string ToolName { get; set; }

        // [Export] public Texture2D ToolSprite { get; set; }
        [Export]
        public int Amount { get; set; } = 1;
    }
}
