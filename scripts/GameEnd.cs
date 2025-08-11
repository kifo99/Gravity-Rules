using System;
using Godot;

public partial class GameEnd : Control
{
    private Label _label;

    public override void _Ready()
    {
      
        _label = GetNode<Label>("VBoxContainer/Label");

        Player player = GetTree().Root.GetNode<Node>("Game").GetNode<Player>("Player");
        if (player != null)
        {
            int score = player.PlayerScore;
            _label.Text = $"Score: {score}";
        }
        else
        {
            _label.Text = "Score: N/A";
            GD.PrintErr("Player node not found");
        }
    }

    private void OnRetryButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
