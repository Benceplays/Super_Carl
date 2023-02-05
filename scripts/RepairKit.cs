using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json;

public class Repairkit : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
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
				new JProperty("Zombie", get_options.zombie),
				new JProperty("CarHP", get_options.carhp),
				new JProperty("Repairkit", get_options.repairkit + 1),
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
        }
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
