using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private KinematicBody2D enemy;
	private float deltaplus;
	private int force = 100000;
	private Timer timer;
	private AnimatedSprite zombie;
	public override void _Ready()
	{
		//enemy = GetNode("Enemy") as KinematicBody2D;
		timer = GetNode("ZombieNyek") as Timer;
		zombie = GetNode("Zombie") as AnimatedSprite;
		zombie.Play("Zombie-run");
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
			timer.Start();
			this.Rotate(1);
			Vector2 velocity = new Vector2(800, 500);
			MoveAndSlide(velocity * force * deltaplus);
			zombie.Stop();
		}
	}
	public void _on_ZombieNyek_timeout()
	{
		QueueFree();
	}

}
