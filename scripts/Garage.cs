using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
public class Garage : Node2D
{ 
    private ConfigBody config;
    private string text;
    private int currentcar;
    public override void _Ready()
    {
		config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
    }
    public override void _Process(float delta)
	{
        config = new ConfigBody();
        text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
    }
    public void _on_Car_pressed(){
        currentcar = 0;
        Save();
    }
     public void _on_Car2_pressed(){
        currentcar = 1;
        Save();
    }
     public void _on_Car3_pressed(){
        currentcar = 2;
        Save();
    }
     public void _on_Car4_pressed(){
        currentcar = 3;
        Save();
    }
    public void Save(){
        config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        JObject options = new JObject(
        new JProperty("CurrentCar", currentcar),
        new JProperty("Money", config.money),
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
    public void _on_Next_pressed(){
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }
}
