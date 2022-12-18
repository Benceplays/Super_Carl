using File = System.IO.File;
using System.IO;
using Godot;
using System;
public class Garage : Node2D
{
    [Export]private PackedScene carTemplate;
    private Node2D carNode;
    public string player_json;

    public override void _Ready()
    {
        player_json = File.ReadAllText(@"scripts/Player.json");
        if (player_json == "")
        {
            WritePlayerData(true);
        }
        carNode = (Node2D)carTemplate.Instance();
        carNode.Set("type",1);
        AddChild(carNode);
        //todo:Olyan Car objektum osztályt csinálni amiket belehet olvasni és a property-ket betölteni.
    }

    public void _on_Next_pressed(){
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void WritePlayerData(bool loadDefault)
    {
        //Amennyiben ures a json fájl =>
        if (loadDefault)
        {
            /*
            var get_playerdata = JsonConvert.DeserializeObject<ConfigBody>(player_json);
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
            */
        }
    }
}