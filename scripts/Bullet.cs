using Godot;
using Newtonsoft.Json;
using System;
using File = System.IO.File;
using System.IO;
using Newtonsoft.Json.Linq;

public class Bullet : KinematicBody2D
{
    private float speed;
    private Vector2 enemyposition;
    private Vector2 irany;
    private float livetime;
    public override void _Ready()
    {
        speed = 2000;
        Rotation = GetAngleTo(enemyposition);
        irany = GlobalPosition.DirectionTo(enemyposition) * speed;
    }
    public override void _Process(float delta)
    {
        livetime += delta;
        if(livetime >= 5){
            QueueFree();
        }
        MoveAndSlide(irany);
    }
    public void _on_Area2D_body_entered(KinematicBody2D enemy){
        if(enemy.IsInGroup("car") == false){
            QueueFree();
        }
        if(enemy.IsInGroup("zombie")){
            enemy.QueueFree();
            File_write();
            QueueFree();
        }
    }
    public void File_write()
	{
		string textplayer = File.ReadAllText(@"scripts/Player.json");
		var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
        JObject options = new JObject(
            new JProperty("CurrentCar", get_optionsplayer.currentcar),
            new JProperty("Money", get_optionsplayer.money),
            new JProperty("Zombie", get_optionsplayer.zombie + 1),
            new JProperty("CarHP", get_optionsplayer.carhp),
			new JProperty("Repairkit", get_optionsplayer.repairkit),
            new JProperty("Is_On_Lift", get_optionsplayer.is_on_lift),
            new JProperty("UnlockedCars", get_optionsplayer.UnlockedCars),
            new JProperty("Cars", get_optionsplayer.Cars),
            new JProperty("Days", get_optionsplayer.Days));
        File.WriteAllText(@"scripts/Player.json", options.ToString());
        using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
    	using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            options.WriteTo(writer);
        }
	}
}
