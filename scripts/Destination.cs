using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json;

public class Destination : Node2D
{
    private Panel reachdestinationpanel;
    private Label bonuslabel;
    private Label distancelabel;
    private Label zombielabel;
    private Label totallabel;
    private int bonus;
    private int zombiemoney;
    private int money;
    private int plusmoney;
    private string path;
    private ConfigFile configfile;
    private int zombienumber;

    public override void _Ready()
    {

    }
    public void _on_Area2D_body_entered(KinematicBody2D body)
    {
        if (body.IsInGroup("car")){
            path = "res://save.cfg";
            configfile = new ConfigFile();
            configfile.Load(path);
            string textplayer = File.ReadAllText(@"scripts/Player.json");
		    var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
            zombienumber = (int)Convert.ToSingle(configfile.GetValue("Default", "Zombie", false));
            money = (int)Convert.ToSingle(configfile.GetValue("Default", "Money", 0));
            bonus = 1000;
            zombiemoney = zombienumber * 50;
            plusmoney = money + zombiemoney + bonus;
            bonuslabel = GetNode("/root/Game/Car/HUD/ReachDestination/Bonus") as Label;
            distancelabel = GetNode("/root/Game/Car/HUD/ReachDestination/Distance") as Label;
            zombielabel = GetNode("/root/Game/Car/HUD/ReachDestination/Zombie") as Label;
            totallabel = GetNode("/root/Game/Car/HUD/ReachDestination/total") as Label;
            reachdestinationpanel = GetNode("/root/Game/Car/HUD/ReachDestination") as Panel;
            bonuslabel.Text = $"Bonus: {bonus}";
            zombielabel.Text = $"Zombie Hit: {zombiemoney}";
		    distancelabel.Text = $"Distance: {money}";
		    totallabel.Text = $"Total Money: {plusmoney}";
            reachdestinationpanel.Visible = true;
            JObject options = new JObject(
                new JProperty("CurrentCar", get_optionsplayer.currentcar),
                new JProperty("Money", get_optionsplayer.money + plusmoney),
                new JProperty("UnlockedCars", get_optionsplayer.UnlockedCars),
                new JProperty("Cars", get_optionsplayer.Cars),
                new JProperty("Days", get_optionsplayer.Days));
            File.WriteAllText(@"scripts/Player.json", options.ToString());
            using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                options.WriteTo(writer);
            }
            GetTree().Paused = true;
        }
    }

    public override void _Process(float delta)
    {

    }
}
