 using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class OutOfPetrol : Panel
{
    private string text;
    public override void _Ready()
    {
        text = File.ReadAllText(@"scripts/Player.json");
    }
    public override void _Process(float delta)
    {
        if(Visible == true){
            var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
            JObject options = new JObject(
                new JProperty("CurrentCar", get_options.currentcar),
                new JProperty("Money", get_options.money),
                new JProperty("UnlockedCars", get_options.UnlockedCars),
                new JProperty("Cars", get_options.Cars),
                new JProperty("Days", (get_options.Days + 1)));
            File.WriteAllText(@"scripts/Player.json", options.ToString());
            using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                options.WriteTo(writer);
            }
        }
    }
    public void _on_Garage_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://scenes/Garage.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
