using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private KinematicBody2D car;
	public override void _Ready()
	{
		car = GetNode("/root/Game/Car") as KinematicBody2D;
	}
	public override void _Process(float delta)
	{
		Vector2 velocity = new Vector2(0, 500);
		var collision = MoveAndCollide(velocity * delta);
		if (collision == null){
		}
		Vector2 moveVector = new Vector2(0, 0);
		moveVector.x -= 20;
		Position += moveVector*10*delta;
	}
	private void _on_ZombieHitbox_body_entered(object body)
	{
		if (body == car){
			GD.Print("asd");
		}
	}

}
