using Godot;
using System;

public class Game : Node2D
{
    private int speed;
    private KinematicBody2D car;

    public override void _Ready()
    {
        car = GetNode("/root/Game/Car/KinematicBody2D") as KinematicBody2D;
        speed = 100;
    }

    public override void _Process(float delta)
    {
      if (Input.IsActionPressed("forward"))
        {
            car.MoveAndSlide(new Vector2(speed, 0));
            GD.Print("Move");
        }    
    }
}
