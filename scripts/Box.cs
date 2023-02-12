using Godot;
using System;

public class Box : RigidBody2D
{
    private AnimatedSprite anim;
    public override void _Ready()
    {
        anim = GetNode("AnimatedSprite") as AnimatedSprite;
    }
    public override void _Process(float delta)
    {
    }
    private void _on_Area2D_body_entered(RigidBody2D car){
        if(car.IsInGroup("car")){
            if(car.LinearVelocity.x > 200){
                anim.Play("default");
            }

        }
    }
    private void _on_AnimatedSprite_animation_finished(){
        QueueFree();
    }
}
