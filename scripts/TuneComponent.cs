using Godot;
using System;
using File = System.IO.File;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;


public class TuneComponent : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public string player_json;
    public Sprite image;
    public string image_path;
    public int car_id;
    public string component;
    public ProgressBar lvlProgress;
    public Label name;
    public int currentLvl;
    [Export] public PackedScene psnotification;
    public Node2D garage;

    public int maxLvl; //itt a maxlvl számolása nem nullától van, tehát ha 0;1;2-es szintek vannak akkor a maximalis szint 3
    public int upgrade_price;
    public override void _Ready()
    {
        garage = (Node2D)GetNode("/root/Garage");
        image = (Sprite)GetNode("Panel/icon");
        lvlProgress = (ProgressBar)GetNode("Panel/lvlProgress");
        lvlProgress.Value = currentLvl;
        name = (Label)GetNode("Panel/name");
        switch (component)
        {
            case "engine":
                maxLvl = 2;
                upgrade_price = 250;

                break;
            case "nitro":
                maxLvl = 3;
                upgrade_price = 500;
                break;
            case "gun":
                maxLvl = 1;
                upgrade_price = 1000;
                break;
            case "petrol":
                maxLvl = 4;
                upgrade_price = 50;
                break;
        }
        image.Texture = (Texture)ResourceLoader.Load("res://assets/images/tune_components/" + component + ".png");
        lvlProgress.MaxValue = maxLvl;
        upgrade_price *= currentLvl + 1;
        name.Text = component.ToUpper() + " (" + upgrade_price + "$)";

    }

    public void _on_buyButton_pressed()
    {
        if (currentLvl < maxLvl)
        {
            player_json = File.ReadAllText(@"scripts/Player.json");
            var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
            if (get_datas.money >= upgrade_price)
            {
                currentLvl++;
                refreshProgressBar();
                writeToJSON();
                name.Text = component.ToUpper() + " (" + upgrade_price + "$)";

                //Elmentése
                JObject options = new JObject(
                new JProperty("CurrentCar", get_datas.currentcar),
                new JProperty("Money", get_datas.money - upgrade_price),
                new JProperty("UnlockedCars", get_datas.UnlockedCars),
                new JProperty("Cars", get_datas.Cars),
                new JProperty("Days", get_datas.Days),
                new JProperty("Maps", get_datas.Maps));
                File.WriteAllText(@"scripts/Player.json", options.ToString());
                using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    options.WriteTo(writer);
                }
                upgrade_price *= currentLvl + 1;


                Node2D notification = (Node2D)psnotification.Instance();
                notification.Set("text", "Sikeresen megvásároltad a tuning elemet!");
                notification.Set("color", "green");
                garage.AddChild(notification);

            }
            else
            {
                GD.Print("nocash");
                Node2D notification = (Node2D)psnotification.Instance();
                notification.Set("text", "Nincs elég pénzed a tuning elem megvételéhez!");
                notification.Set("color", "red");
                garage.AddChild(notification);
            }
        }
    }

    public void refreshStats()
    {
        switch (component)
        {
            case "engine":
                maxLvl = 2;
                upgrade_price = 250;
                break;
            case "nitro":
                maxLvl = 3;
                upgrade_price = 500;
                break;
            case "gun":
                maxLvl = 1;
                upgrade_price = 1000;
                break;
            case "petrol":
                maxLvl = 4;
                upgrade_price = 50;
                break;
        }
        upgrade_price *= currentLvl + 1;
        name.Text = component.ToUpper() + " (" + upgrade_price + "$)";
    }

    public void refreshProgressBar()
    {
        lvlProgress.Value = currentLvl;
    }

    public override void _Process(float delta)
    {
        refreshProgressBar();
        refreshStats();
    }

    public void writeToJSON()
    {
        string tunings = File.ReadAllText(@"scripts/Tunings.json");
        string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása
        for (int i = 0; i < tunings_split.Length; i++)
        {
            Dictionary<string, int> currentDic =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
            Dictionary<string, int> kocsi = new Dictionary<string, int>();
            if (car_id == currentDic["id"])
            {
                kocsi.Add("id", car_id);
                kocsi.Add("engine", currentDic["engine"]);
                kocsi.Add("gun", currentDic["gun"]);
                kocsi.Add("petrol", currentDic["petrol"]);
                kocsi.Add("nitro", currentDic["nitro"]);
                //A rövidítés érdekében beállítunk mindent alap értékre és utána csak a bizonyos tuningkomponenset írjuk át a currentLvlre.
                kocsi[component] = currentLvl;
            }
            else
            {
                kocsi = currentDic;
            }
            if (i != 0)
            {
                File.AppendAllText(@"scripts/Tunings.json", "\n" + JsonConvert.SerializeObject(kocsi));
            }
            else
            {
                File.WriteAllText(@"scripts/Tunings.json", JsonConvert.SerializeObject(kocsi));
            }
            //GD.Print(tunings_split[i]);
        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
