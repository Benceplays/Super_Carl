using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;

public class SoundController : Node2D
{
    private AudioStreamPlayer2D music;
    public override void _Ready()
    {
        string text = File.ReadAllText(@"scripts/Options.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);

        music = GetNode("Music") as AudioStreamPlayer2D;
        music.VolumeDb = get_options.musicvolume;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
