using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class MapSelector : Node2D
{
    private string path;
    private ConfigFile config;
    private string textplayer;
    public override void _Ready()
    {
        path = "res://save.cfg";
		config = new ConfigFile();
        
        textplayer = File.ReadAllText(@"scripts/Player.json");
    }

    public void _on_Map1_pressed(){
        var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
        if(get_optionsplayer.Maps.Contains(1)){
		    config.SetValue("Default", "MapNumber", 1);
		    config.Save(path);
            GetTree().ChangeScene("res://scenes/Game.tscn");
        }
    }

    public void _on_Map2_pressed(){
        var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
        if(get_optionsplayer.Maps.Contains(2)){
		    config.SetValue("Default", "MapNumber", 2);
		    config.Save(path);
            GetTree().ChangeScene("res://scenes/Game.tscn");
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
