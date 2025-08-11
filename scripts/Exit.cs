using System;
using Godot;

public partial class Exit : Control
{
    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
