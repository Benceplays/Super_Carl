using Godot;
using System;

public class Box : RigidBody2D
{
    public override void _Ready()
    {
        
    }
    // A kocsi által ért impulzus mértetől függöen törne szét vagy nem
    public override void _Process(float delta)
    {
        //GD.Print(AppliedForce);
        //GD.Print(AppliedTorque);
    }
    private void _on_Area2D_body_entered(RigidBody2D car){
        if(car.IsInGroup("car")){
            GD.Print(car.AppliedTorque);
            GD.Print(AppliedForce);
        }
    }
}
