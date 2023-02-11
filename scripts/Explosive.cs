using Godot;
using System;

public class Explosive : RigidBody2D
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
            GD.Print(car.LinearVelocity.x);
            if(car.LinearVelocity.x > 600){
                anim.Play("default");
            }

        }
    }
    private void _on_AnimatedSprite_animation_finished(){
        QueueFree();
        // Egy robbanás animcáió kell még
        // Egy nagyobb Area2D kell amiben lévő dobozok, lécek, robbanó hordok is felrobbanak (lejátsza az animációt és eltűnnek)
    }
}
