using Godot;
using Newtonsoft.Json;
using System;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json.Linq;

public class Bullet : KinematicBody2D
{
	private float speed;
	private Vector2 enemyposition;
	private Vector2 irany;
	private float livetime;
	public String path;
	public ConfigFile config;
	public override void _Ready()
	{
		speed = 2000;
		Rotation = GetAngleTo(enemyposition);
		irany = GlobalPosition.DirectionTo(enemyposition) * speed;
	}
	public override void _Process(float delta)
	{
		livetime += delta;
		if(livetime >= 5){
			QueueFree();
		}
		MoveAndSlide(irany);
	}
	public void _on_Area2D_body_entered(KinematicBody2D enemy){
		if(enemy.IsInGroup("car") == false){
			QueueFree();
		}
		if(enemy.IsInGroup("zombie")){
			enemy.QueueFree();
			File_write();
			QueueFree();
		}
	}
	public void File_write()
	{
		string textplayer = File.ReadAllText(@"scripts/Player.json");
		var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
		JObject options = new JObject(
			new JProperty("CurrentCar", get_optionsplayer.currentcar),
			new JProperty("Money", get_optionsplayer.money),
			new JProperty("UnlockedCars", get_optionsplayer.UnlockedCars),
			new JProperty("Cars", get_optionsplayer.Cars),
			new JProperty("Days", get_optionsplayer.Days),
			new JProperty("Maps", get_optionsplayer.Maps));
		File.WriteAllText(@"scripts/Player.json", options.ToString());
		using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
		using (JsonTextWriter writer = new JsonTextWriter(file))
		{
			options.WriteTo(writer);
		}
		path = "res://save.cfg"; // res vagy user:
		config = new ConfigFile();
		config.Load(path);
		config.SetValue("Default", "Zombie", Convert.ToSingle(config.GetValue("Default", "Zombie", 0)) + 1);
		config.SetValue("Default", "CarHP", Convert.ToSingle(config.GetValue("Default", "CarHP", 0)));
		config.SetValue("Default", "Repairkit", Convert.ToSingle(config.GetValue("Default", "Repairkit", 0)));
		config.SetValue("Default", "Is_On_Lift", Convert.ToSingle(config.GetValue("Default", "Is_On_Lift", false)));
		config.Save(path);
	}
}
