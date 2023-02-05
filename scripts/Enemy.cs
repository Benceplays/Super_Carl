using Godot;
using Newtonsoft.Json;
using System;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json.Linq;

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
			File_write();
		}
	}
	public void File_write()
	{
		string textplayer = File.ReadAllText(@"scripts/Player.json");
		var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
        JObject options = new JObject(
            new JProperty("CurrentCar", get_optionsplayer.currentcar),
            new JProperty("Money", get_optionsplayer.money),
            new JProperty("Zombie", get_optionsplayer.zombie + 1),
			new JProperty("CarHP", get_optionsplayer.carhp - 20),
			new JProperty("Repairkit", get_optionsplayer.repairkit),
            new JProperty("Is_On_Lift", get_optionsplayer.is_on_lift),
            new JProperty("UnlockedCars", get_optionsplayer.UnlockedCars),
            new JProperty("Cars", get_optionsplayer.Cars),
            new JProperty("Days", get_optionsplayer.Days));
        File.WriteAllText(@"scripts/Player.json", options.ToString());
        using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
    	using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            options.WriteTo(writer);
        }
	}
	public void _on_ZombieNyek_timeout()
	{
		QueueFree();
	}

}
