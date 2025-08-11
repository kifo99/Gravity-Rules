using System;
using Godot;

public partial class MainMenu : Control
{
    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/game.tscn");
    }

    private void OnSettingsButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/settings_menu.tscn");
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
