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
    }

    public override void _Process(float delta)
    {  
        config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        money = (int) car.Position.x / 100;
        moneylabel.Text = "Money: " + money;
    }
     public void Save(){
        config = new ConfigBody();
		text = File.ReadAllText(@"scripts/Options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
       JObject options = new JObject(
       new JProperty("CurrentCar", config.currentcar),
       new JProperty("Money", money)
	    );
	    File.WriteAllText(@"scripts/Options.json", options.ToString());
	    using (StreamWriter file = File.CreateText(@"scripts/Options.json"))
	    using (JsonTextWriter writer = new JsonTextWriter(file))
	    {
	    	options.WriteTo(writer);
	    }
	}
}
