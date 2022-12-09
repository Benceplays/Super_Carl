using Godot;
using System;

public class Game : Node2D
{
    [Export] public int money;
    private Label moneylabel;
    private Label fpslabel;

    public override void _Ready()
    {
        moneylabel = GetNode("/root/Game/Car/HUD/Money") as Label;
        fpslabel = GetNode("/root/Game/Car/HUD/Fps") as Label;
    }

    public override void _Process(float delta)
    {  
        moneylabel.Text = $"Money: {money}";
        fpslabel.Text = $"{Convert.ToInt32(1 / delta)} FPS";
    }
}
