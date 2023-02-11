using Godot;
using System;

public class Explosive : RigidBody2D
{
    private AnimatedSprite anim;
    private Sprite spritekep;
    private bool explosion;
    private Area2D explosionarea;
    public override void _Ready()
    {
        anim = GetNode("AnimatedSprite") as AnimatedSprite;
        spritekep = GetNode("Sprite") as Sprite;
        explosionarea = GetNode("ExplosionArea") as Area2D;
    }
    public override void _Process(float delta)
    {
    }
    private void _on_Area2D_body_entered(RigidBody2D car){
        if(car.IsInGroup("car")){
            if(car.LinearVelocity.x > 150){
                spritekep.Visible = false;
                anim.Visible = true;
                anim.Play("default");
            }

        }
    }
    private void _on_ExplosionArea_body_entered(RigidBody2D wood){
        if(explosion){
            if(wood.IsInGroup("zombie") || wood.IsInGroup("plank")){
                wood.QueueFree();
            }
            if(wood.IsInGroup("box")){
                wood.GetNode<AnimatedSprite>("AnimatedSprite").Play("default");
            }
            if(wood.IsInGroup("explosive")){
                wood.GetNode<Sprite>("Sprite").Visible = false;
                wood.GetNode<AnimatedSprite>("AnimatedSprite").Visible = true;
                wood.GetNode<AnimatedSprite>("AnimatedSprite").Play("default");
            }
            QueueFree();
        }
    }
    private void _on_AnimatedSprite_animation_finished(){
        explosion = true;
        explosionarea.Monitoring = true;
        // Egy robbanás animcáió kell még
        // Egy nagyobb Area2D kell amiben lévő dobozok, lécek, robbanó hordok is felrobbanak (lejátsza az animációt és eltűnnek)
    }
    
}
