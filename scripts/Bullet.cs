using Godot;
using System;

public class Bullet : KinematicBody2D
{
    private float speed;
    private Vector2 enemyposition;
    private Vector2 irany;
    private float livetime;
    public override void _Ready()
    {
        speed = 2000;
        Rotation = GetAngleTo(enemyposition);
        irany = GlobalPosition.DirectionTo(enemyposition) * speed;
    }
    public override void _Process(float delta)
    {
        livetime += delta;
        if(livetime >= 5){
            QueueFree();
        }
        MoveAndSlide(irany);
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
