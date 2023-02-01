using Godot;
using System;

public class Bullet : KinematicBody2D
{
    public float speed;
    public Vector2 irany;
    private float livetime;
    private Area2D bulletarea;
    public override void _Ready()
    {
       // var target = Vector2.Zero;
        bulletarea = GetNode("Area2D") as Area2D;
        speed = 5;
        /*target = GetGlobalMousePosition();
        bulletarea.Rotation = GetAngleTo(target);
        irany = GlobalPosition.DirectionTo(target) * speed;*/
    }
    public override void _Process(float delta)
    {
        livetime += delta;
        //bulletarea.Position += irany;
        if(livetime >= 5){
            QueueFree();
        }
    }
    public void _on_Area2D_body_entered(KinematicBody2D enemy){
        if(enemy.IsInGroup("car") == false){
            QueueFree();
        }
        if(enemy.IsInGroup("zombie")){
            enemy.QueueFree();
            QueueFree();
        }
    }
}
