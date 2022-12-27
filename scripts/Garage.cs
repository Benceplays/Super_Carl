using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Garage : Node2D
{
    [Export]private PackedScene carTemplate;
    private Node2D carNode;
    public string player_json;

    public override void _Ready()
    {
        player_json = File.ReadAllText(@"scripts/Tunings.json");
        if (player_json == "")
        {
            WritePlayerData(true);
        }
        carNode = (Node2D)carTemplate.Instance();
        AddChild(carNode);
        //todo:Olyan Car objektum osztályt csinálni amiket belehet olvasni és a property-ket betölteni.
    }

    public void _on_Next_pressed(){
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void WritePlayerData(bool loadDefault)
    {
        string text = File.ReadAllText(@"scripts/Player.json");
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(text);
        
        string text2 = File.ReadAllText(@"scripts/Tunings.json");
        var get_tunings = JsonConvert.DeserializeObject<ConfigBody>(text2);
        
        //Amennyiben ures a json fájl =>
        if (loadDefault)
        {
            int index = -1;
            foreach (var car in get_datas.Cars)
            {
                index++;
                Dictionary<string, int> kocsi = new Dictionary<string, int>();
                kocsi.Add("id",car);
                kocsi.Add("engine",0);
                kocsi.Add("gun",0);
                kocsi.Add("petrol",0);
                kocsi.Add("wheel", 0);
                if (index != 0)
                {
                    File.AppendAllText(@"scripts/Tunings.json","\n" + JsonConvert.SerializeObject(kocsi));
                }
                else
                {
                    File.AppendAllText(@"scripts/Tunings.json", JsonConvert.SerializeObject(kocsi));
                }
            }
        }
    }
}