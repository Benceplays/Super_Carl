using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json;

public class Repairkit : RigidBody2D
{
    private string path;
	private ConfigFile config;
    public override void _Ready()
    {
        
    }


    public void _on_RepairkitArea_body_entered(KinematicBody2D body)
    {
        if (body.IsInGroup("car")){
			string text = File.ReadAllText(@"scripts/Player.json");
			var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);

			JObject options = new JObject(
				new JProperty("CurrentCar", get_options.currentcar),
				new JProperty("Money", get_options.money),
				new JProperty("UnlockedCars", get_options.UnlockedCars),
				new JProperty("Cars", get_options.Cars),
				new JProperty("Days", get_options.Days));
			File.WriteAllText(@"scripts/Player.json", options.ToString());
			using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
			using (JsonTextWriter writer = new JsonTextWriter(file))
			{
				options.WriteTo(writer);
			}
			QueueFree();
			path = "res://save.cfg"; // res vagy user:
			config = new ConfigFile();
			config.Load(path);
			config.SetValue("Default", "Zombie", Convert.ToSingle(config.GetValue("Default", "Zombie", 0)));
			config.SetValue("Default", "CarHP", Convert.ToSingle(config.GetValue("Default", "CarHP", 0)));
			config.SetValue("Default", "Repairkit", Convert.ToSingle(config.GetValue("Default", "Repairkit", 0)) + 1);
			config.SetValue("Default", "Is_On_Lift", Convert.ToSingle(config.GetValue("Default", "Is_On_Lift", false)));
			config.Save(path);
        }
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
