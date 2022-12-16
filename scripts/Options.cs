using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;

public class Options : Node2D
{
    public HSlider musicvolume;
    public HSlider soundeffectvolume;
    public HSlider uivolume;
    public CheckBox fpsbutton;
    public CheckBox vsyncbutton;
    public OptionButton fpsselect;
    public OptionButton displayselect;
    
    
    public override void _Ready()
    {
        musicvolume = GetNode("/root/Options/Audio/MusicVolume") as HSlider;
        soundeffectvolume = GetNode("/root/Options/Audio/SoundEffectVolume") as HSlider;
        uivolume = GetNode("/root/Options/Audio/UIVolume") as HSlider;
        fpsbutton = GetNode("/root/Options/Display/FPS_ON") as CheckBox;
        vsyncbutton = GetNode("/root/Options/Display/Vsync") as CheckBox;
        fpsselect = GetNode("/root/Options/Display/FPSTarget") as OptionButton;
        displayselect = GetNode("/root/Options/Display/DisplayMode") as OptionButton;
    }

    public void _on_TextureButton_pressed()
    {
        ConfigBody config = new ConfigBody();
        string text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        JObject options = new JObject(
            new JProperty("CurrentCar", get_options.currentcar),
            new JProperty("Money", get_options.money),
            new JProperty("musicvolume", (int)musicvolume.Value),
            new JProperty("soundeffectvolume", (int)soundeffectvolume.Value),
            new JProperty("uivolume", (int)uivolume.Value),
            new JProperty("fpsison", (bool)fpsbutton.Pressed),
            new JProperty("fps", (int)musicvolume.Value),  
            new JProperty("vsync", (bool)vsyncbutton.Pressed),
            new JProperty("displaymode", (int)musicvolume.Value)
        );
        File.WriteAllText(@"scripts/Options.json", options.ToString());
        using (StreamWriter file = File.CreateText(@"scripts/Options.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            options.WriteTo(writer);
        }
    }
    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
