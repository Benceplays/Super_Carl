using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        Vector2 velocity = new Vector2(0, 100);
        var collision = MoveAndCollide(velocity * delta);
        if (collision != null)
        {
            GD.Print("I collided with ", ((Node)collision.Collider).Name);
            velocity = MoveAndSlide(velocity);
        }

        // Using MoveAndSlide.
        for (int i = 0; i < GetSlideCount(); i++)
        {
            collision = GetSlideCollision(i);
            GD.Print("I collided with ", ((Node)collision.Collider).Name);
        }
    }
}
