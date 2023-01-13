using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Garage : Node2D
{
    public string tuningjson;
    public string player_json;
    public Label money_label;
    public Sprite car0;
    public Sprite car1;
    public Sprite car2;
    public Sprite[] car_sprites;
    public override void _Ready()
    {
        tuningjson = File.ReadAllText(@"scripts/Tunings.json");
        if (tuningjson == "")
        {
            WritePlayerData(true);
        }
        player_json = File.ReadAllText(@"scripts/Player.json");
        money_label = (Label)GetNode("top_Hud/money");
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
        money_label.Text = get_datas.money + "$";

        car0 = (Sprite)GetNode("mid_hud/car0");
        car1 = (Sprite)GetNode("mid_hud/car1");
        car2 = (Sprite)GetNode("mid_hud/car2");

        car_sprites = new Sprite[3];
        car_sprites[0] = car0;
        car_sprites[1] = car1;
        car_sprites[2] = car2;
    }

    public void _on_Next_pressed(){
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void _on_Back_pressed()
    {
        for (int i = 0; i < car_sprites.Length; i++)
        {
            car_sprites[i].SetPosition(new Vector2(car_sprites[i].Position.x - 2000, car_sprites[i].Position.y));
            GD.Print(i + " kocsi:" + car_sprites[i].Position.x);
        }
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