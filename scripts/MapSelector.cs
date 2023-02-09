using Godot;
using System;

public class MapSelector : Node2D
{
    private string path;
    private ConfigFile config;
    public override void _Ready()
    {
        path = "res://save.cfg";
		config = new ConfigFile();
    }

    public void _on_Map1_pressed(){
		config.SetValue("Default", "MapNumber", 1);
		config.Save(path);
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void _on_Map2_pressed(){
		config.SetValue("Default", "MapNumber", 2);
		config.Save(path);
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
