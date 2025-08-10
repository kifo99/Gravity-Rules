using System;
using Game.Physics;
using Godot;

public partial class CardSwitcher : Area2D
{
    [Export]
    public CardType NewCardType;

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.ApplyCardType(NewCardType);
            GD.Print("Card type switched to: " + NewCardType.Name);
        }
    }
}
