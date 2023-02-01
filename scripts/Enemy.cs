using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private KinematicBody2D enemy;
	private float deltaplus;
	public override void _Ready()
	{
		//enemy = GetNode("Enemy") as KinematicBody2D;
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
		deltaplus = delta;
	}
	private void _on_ZombieHitbox_body_entered(KinematicBody2D body)
	{
		if (body.IsInGroup("car")){
		GD.Print("idiota milan lefut");
		Vector2 velocity = new Vector2(50000000, 0);
		MoveAndSlide(velocity * deltaplus);
		}
	}

}
