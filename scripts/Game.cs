using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;

public class Game : Node2D
{
    [Export] public PackedScene psCar;
    [Export] public PackedScene psBus;
    [Export] public PackedScene psHUD;
    private Label moneylabel;
    private Label fpslabel;
    private ConfigBody config;
    private string text;
    private float time;
    private int money;
    private RigidBody2D car;
    public bool fps_is_on = false;

    public override void _Ready()
    {
		config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);

		CanvasLayer hud = (CanvasLayer)psHUD.Instance();
        switch(config.currentcar){
            case 0:
                Node2D car = (Node2D)psCar.Instance();
                car.Position = new Vector2(0, 0);
                AddChild(car);
                car.AddChild(hud);
                break;
            case 1:
                Node2D bus = (Node2D)psBus.Instance();
                bus.Position = new Vector2(0, 0);
                AddChild(bus);
                bus.AddChild(hud);
                break;
        }
        
        moneylabel = GetNode("Car/HUD/money") as Label;
        car = GetNode("Car") as RigidBody2D;
        //vsync trun
        if(get_options.vsync == true)OS.VsyncEnabled = true; else OS.VsyncEnabled = false;
        //fpstarget set
        if (get_options.fpsison)
        {
	        fps_is_on = true;
	        fpslabel = GetNode("Car/HUD/fps") as Label;

	        switch (get_options.fps)
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
	    
        config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        money = (int) car.Position.x / 100;
        moneylabel.Text = "Money: " + money;
        //fps to screen
        if (fps_is_on)
        {
	        fpslabel.Text = $"{Convert.ToInt32(1 / delta)} FPS";
        }
    }
     public void Save(){
        config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
       JObject options = new JObject(
       new JProperty("CurrentCar", config.currentcar),
       new JProperty("Money", money),
       new JProperty("musicvolume", (int)get_options.musicvolume),
       new JProperty("soundeffectvolume", (int)get_options.soundeffectvolume),
       new JProperty("uivolume", (int)get_options.uivolume),
       new JProperty("fpsison", (bool)get_options.fpsison),
       new JProperty("fps", (int)get_options.fps),
       new JProperty("vsync", (bool)get_options.vsync),
       new JProperty("displaymode", (int)get_options.displaymode)
	    );
	    File.WriteAllText(@"scripts/Options.json", options.ToString());
	    using (StreamWriter file = File.CreateText(@"scripts/Options.json"))
	    using (JsonTextWriter writer = new JsonTextWriter(file))
	    {
	    	options.WriteTo(writer);
	    }
	}
}
