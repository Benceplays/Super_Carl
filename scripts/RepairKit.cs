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
    public void _on_RepairkitArea_body_entered(KinematicBody2D body)
    {
        if (body.IsInGroup("car")){
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
}
