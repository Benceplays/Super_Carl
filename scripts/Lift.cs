using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json;

public class Lift : KinematicBody2D
{
    private float deltatime;
    private bool lift_is_on;
    private string path;
    private ConfigFile config;
    public override void _Ready()
    {

    }
    public void _on_LiftUpArea_body_entered(KinematicBody2D body)
    {
        if (body.IsInGroup("car"))
        {   
            lift_is_on = true;
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
            config.SetValue("Default", "Is_On_Lift", true);
            config.Save(path);
        }
    }

    public void _on_LiftOutArea_body_exited(KinematicBody2D body)
    {
        if (body.IsInGroup("car"))
        {
           lift_is_on = false;
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
            config.SetValue("Default", "CarHP", Convert.ToSingle(config.GetValue("Default", "CarHP", 0)) - 20);
            config.SetValue("Default", "Repairkit", Convert.ToSingle(config.GetValue("Default", "Repairkit", 0)));
            config.SetValue("Default", "Is_On_Lift", false);
            config.Save(path);
        }
    }

    public override void _Process(float delta)
    {
        if(lift_is_on == true)
        {
            if(this.Position != new Vector2(0, 1000))
            {
                this.MoveLocalY(-(100 * delta));
            }
        }
    }
}
