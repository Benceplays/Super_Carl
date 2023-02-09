using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json;

public class Game : Node2D
{
    [Export] public PackedScene psMap1;
    [Export] public PackedScene psMap2;
    [Export] public PackedScene psCar;
    [Export] public PackedScene psBus;
    [Export] public PackedScene psHUD;
    [Export] public PackedScene psRiot;
    [Export] public PackedScene psEnemy;
    private Label fpslabel;
    private ConfigBody config;
    private string text;
    private string OptionsText;
    private string tunings;
    private float time;
    private RigidBody2D car;
    public bool fps_is_on = false;
    private string path;
    private ConfigFile configfile;
    public override void _Ready()
    {
        path = "res://save.cfg";
		configfile = new ConfigFile();
        configfile.Load(path);
        switch(Convert.ToSingle(configfile.GetValue("Default", "MapNumber", 0))){
            case 1:
                Node2D map1 = (Node2D)psMap1.Instance();
                AddChild(map1);
                break;
            case 2:
                Node2D map2 = (Node2D)psMap2.Instance();
                AddChild(map2);
                break;
        }
        
        Random rnd = new Random();
        for (int i = 0; i < 30; i++)
        {
            Node2D enemy = (Node2D)psEnemy.Instance();
            enemy.Position = new Vector2(rnd.Next(3000, 100000), -rnd.Next(10000, 15000));
            AddChild(enemy);
        }

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
        configfile = new ConfigFile();
        configfile.Load(path);
        configfile.SetValue("Default", "Zombie", 0);
        configfile.SetValue("Default", "CarHP", 100);
        configfile.SetValue("Default", "Repairkit", 10);
        configfile.Save(path);

        text = File.ReadAllText(@"scripts/Player.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);

        OptionsText = File.ReadAllText(@"scripts/Options.json");
        var OptionsOption = JsonConvert.DeserializeObject<ConfigBody>(OptionsText);
        // Tunings json beolvasása és annak használata

        GD.Print(OptionsOption.fpsison);


        CanvasLayer hud = (CanvasLayer)psHUD.Instance();
        switch (get_options.currentcar)
        {
            case 0:
                Node2D car = (Node2D)psCar.Instance();
                car.Position = new Vector2(0, 0);
                car.Set("id", 0);
                AddChild(car);
                car.AddChild(hud);
                break;
            case 1:
                Node2D bus = (Node2D)psBus.Instance();
                bus.Position = new Vector2(0, 0);
                bus.Set("id", 1);
                AddChild(bus);
                bus.AddChild(hud);
                break;
            case 2:
                Node2D riot = (Node2D)psRiot.Instance();
                riot.Position = new Vector2(0, 0);
                riot.Set("id", 2);
                AddChild(riot);
                riot.AddChild(hud);
                break;

        }
        /*foreach(var item in get_options.UnlockedCars){
            GD.Print(item);
        }*/
        car = GetNode("Car") as RigidBody2D;
        //vsync trun
        if (OptionsOption.vsync == true) OS.VsyncEnabled = true; else OS.VsyncEnabled = false;
        //fpstarget set
        if (OptionsOption.fpsison)
        {
            fps_is_on = true;
            fpslabel = GetNode("Car/HUD/fps") as Label;

            switch (OptionsOption.fps)
            {
                case 0:
                    Engine.TargetFps = 30;
                    break;

                case 1:
                    Engine.TargetFps = 60;
                    break;

                case 2:
                    Engine.TargetFps = 120;
                    break;

                case 3:
                    Engine.TargetFps = 240;
                    break;

                case 4:
                    Engine.TargetFps = 360;
                    break;

                case 5:
                    Engine.TargetFps = 10000;
                    break;

            }
        }
        else
        {
            fps_is_on = false;
        }

    }

    public override void _Process(float delta)
    {
        text = File.ReadAllText(@"scripts/Player.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        //fps to screen
        if (fps_is_on)
        {
            fpslabel.Text = $"{Convert.ToInt32(1 / delta)} FPS";
        }
    }
}
