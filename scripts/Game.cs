using Godot;
using System;

public class Game : Node2D
{
    [Export] public int speed;
    [Export] public int money;
    private Label moneylabel;

    public override void _Ready()
    {
        moneylabel = GetNode("/root/Game/Car/HUD/Money") as Label;
        speed = 100;
    }

    public override void _Process(float delta)
    {  
        moneylabel.Text = $"Money: {money}";
    }
}
