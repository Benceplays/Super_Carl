using Godot;
using System;

public class Destination : Node2D
{
    private Panel reachdestinationpanel;
    public override void _Ready()
    {
    }
    public void _on_Area2D_body_entered(KinematicBody2D body)
    {
        if (body.IsInGroup("car")){
            reachdestinationpanel = GetNode("/root/Game/Car/HUD/ReachDestination") as Panel;
            reachdestinationpanel.Visible = true;
            GetTree().Paused = true;
        }
    }

    public override void _Process(float delta)
    {

    }
}
