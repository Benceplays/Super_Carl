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
    public int displayindex;
    public int fpstarget;


    public override void _Ready()
    {
        musicvolume = GetNode("/root/Options/Audio/MusicVolume") as HSlider;
        soundeffectvolume = GetNode("/root/Options/Audio/SoundEffectVolume") as HSlider;
        uivolume = GetNode("/root/Options/Audio/UIVolume") as HSlider;
        fpsbutton = GetNode("/root/Options/Display/FPS_ON") as CheckBox;
        vsyncbutton = GetNode("/root/Options/Display/Vsync") as CheckBox;
        fpsselect = GetNode("/root/Options/Display/FPSTarget") as OptionButton;
        displayselect = GetNode("/root/Options/Display/DisplayMode") as OptionButton;
        
        string text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        
        musicvolume.Value = get_options.musicvolume;
        soundeffectvolume.Value = get_options.soundeffectvolume;
        uivolume.Value = get_options.uivolume;
        fpsbutton.Pressed = get_options.fpsison;
        vsyncbutton.Pressed = get_options.vsync;
        
        fpsselect.AddItem("30");
        fpsselect.AddItem("60");
        fpsselect.AddItem("120");
        fpsselect.AddItem("240");
        fpsselect.AddItem("360");
        fpsselect.AddItem("Unlimited");
        
        displayselect.AddItem("Bordless");
        displayselect.AddItem("Fullscreen");
        displayselect.AddItem("Bordless Fulscreen");
        
        fpsselect.Selected = get_options.fps;
        displayselect.Selected = get_options.displaymode;
        displayindex = get_options.displaymode;
        fpstarget = get_options.fps;
    }

    public void _on_FPSTarget_item_selected(int index)
    {
        switch (index)
        {
            case 0:
                fpstarget = 0;
                break;
                
            case 1:
                fpstarget = 1;
                break;
                
            case 2:
                fpstarget = 2;
                break;
                
            case 3:
                fpstarget = 3;
                break;
                
            case 4:
                fpstarget = 4;
                break;
                
            case 5:
                fpstarget= 5;
                break;
        }
    }

    public void _on_DisplayMode_item_selected(int index)
    {
        switch (index)
        {
            case 0:
                displayindex = 0;
                break;
            case 1:
                displayindex = 1;
                break;
            case 2:
                displayindex = 2;
                break;
        }
    }

    public void _on_Save_pressed()
    {
        string text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        JObject options = new JObject(
            new JProperty("musicvolume", (int)musicvolume.Value),
            new JProperty("soundeffectvolume", (int)soundeffectvolume.Value),
            new JProperty("uivolume", (int)uivolume.Value),
            new JProperty("fpsison", (bool)fpsbutton.Pressed),
            new JProperty("fps", (int)fpstarget),
            new JProperty("vsync", (bool)vsyncbutton.Pressed),
            new JProperty("displaymode", (int)displayindex)
        );
        File.WriteAllText(@"scripts/Options.json", options.ToString());
        using (StreamWriter file = File.CreateText(@"scripts/Options.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            options.WriteTo(writer);
        }

        GetTree().ChangeScene("res://scenes/Menu.tscn");
    }
    

    public override void _Process(float delta)
    {
        if (fpsbutton.Pressed) { fpsbutton.Text = "ON"; }
        else { fpsbutton.Text = "OFF"; }
        if (vsyncbutton.Pressed) { vsyncbutton.Text = "ON"; }
        else { vsyncbutton.Text = "OFF"; }
    }
}
