using Godot;
using System;
using System.Collections.Generic;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json;

public class Game : Node2D
{
    [Export] public PackedScene psCar;
    [Export] public PackedScene psBus;
    [Export] public PackedScene psHUD;
    private Label moneylabel;
    private Label fpslabel;
    private ConfigBody config;
    private string text;
    private string OptionsText;
    private string tunings;
    private float time;
    private int money;
    private RigidBody2D car;
    public bool fps_is_on = false;
    private Timer riptimer;

    public override void _Ready()
    {
        riptimer = GetNode("RIPTimer") as Timer;
	    text = File.ReadAllText(@"scripts/Player.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        
	    OptionsText = File.ReadAllText(@"scripts/Options.json");
		var OptionsOption = JsonConvert.DeserializeObject<ConfigBody>(OptionsText);
		// Tunings json beolvasása és annak használata

        GD.Print(OptionsOption.fpsison);

		
		CanvasLayer hud = (CanvasLayer)psHUD.Instance();
        switch(get_options.currentcar){
            case 0:
                Node2D car = (Node2D)psCar.Instance();
                car.Position = new Vector2(0, 0);
                car.Set("id",0);
                AddChild(car);
                car.AddChild(hud);
                break;
            case 1:
                Node2D bus = (Node2D)psBus.Instance();
                bus.Position = new Vector2(0, 0);
                bus.Set("id",1);
                AddChild(bus);
                bus.AddChild(hud);
                break;
        }
        /*foreach(var item in get_options.UnlockedCars){
            GD.Print(item);
        }*/

        moneylabel = GetNode("Car/HUD/money") as Label;
        car = GetNode("Car") as RigidBody2D;
        riptimer.Connect("timeout", car,"_on_RIPTimer_timeout");
        //vsync trun
        if(OptionsOption.vsync == true)OS.VsyncEnabled = true; else OS.VsyncEnabled = false;
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
        money = (int) car.Position.x / 100;
        moneylabel.Text = "Money: " + money;
        //fps to screen
        if (fps_is_on)
        {
	        fpslabel.Text = $"{Convert.ToInt32(1 / delta)} FPS";
        }
    }
}
