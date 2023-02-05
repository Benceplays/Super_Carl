using Godot;
using System;

public class Lift : KinematicBody2D
{
    private float deltatime;
    private bool lift_is_on;
    public override void _Ready()
    {

    }
    public void _on_LiftUpArea_body_entered(KinematicBody2D body)
    {
        if (body.IsInGroup("car"))
        {   
            lift_is_on = true;
        }
    }
    public override void _Process(float delta)
    {
        if(lift_is_on == true)
        {
            if(this.Position != new Vector2(0, 1000))
            {
                this.MoveLocalY(-(50 * delta));
            }
        }
    }
}
