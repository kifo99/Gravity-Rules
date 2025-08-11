using System;
using Godot;

public partial class SettingsMenu : Control
{
    private HSlider _volumeSlider;
    private CheckBox _fullscreenCheckBox;
    private Button _backButton;

    public override void _Ready()
    {
        _volumeSlider = GetNode<HSlider>(
            "MarginContainer/VBoxContainer/MasterVolumeSlider/HSlider"
        );
        _fullscreenCheckBox = GetNode<CheckBox>(
            "MarginContainer/VBoxContainer/FullscreenCheckBox/CheckBox"
        );
        _backButton = GetNode<Button>("MarginContainer/VBoxContainer/Button");

        float volumeDb = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master"));
        _volumeSlider.Value = DbToLinear(volumeDb);

        _fullscreenCheckBox.ButtonPressed =
            DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;

        _volumeSlider.ValueChanged += OnVolumeChanged;
        _fullscreenCheckBox.Toggled += OnFullscreenToggled;
        _backButton.Pressed += OnBackButtonPressed;
    }

    private float LinearToDb(float linear)
    {
        if (linear == 0)
            return -80f;
        return 20f * MathF.Log10(linear);
    }

    private float DbToLinear(float db)
    {
        return MathF.Pow(10f, db / 20f);
    }

    private void OnVolumeChanged(double value)
    {
        float linearValue = (float)value;
        float db = LinearToDb(linearValue);
        int masterBus = AudioServer.GetBusIndex("Master");
        AudioServer.SetBusVolumeDb(masterBus, db);
    }

    private void OnFullscreenToggled(bool isFullscreen)
    {
        if (isFullscreen)
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        else
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }

    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }
}
