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
                new JProperty("Zombie", get_optionsplayer.zombie),
                new JProperty("CarHP", get_optionsplayer.carhp),
                new JProperty("Repairkit", get_optionsplayer.repairkit),
                new JProperty("Is_On_Lift", true),
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
                new JProperty("Zombie", get_optionsplayer.zombie),
                new JProperty("CarHP", get_optionsplayer.carhp),
                new JProperty("Repairkit", get_optionsplayer.repairkit),
                new JProperty("Is_On_Lift", false),
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
