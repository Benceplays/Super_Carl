using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Garage : Node2D
{
    public string tuningjson;
    public string player_json;
    public Label money_label;
    public Label days_label;
    public Sprite car0;
    public Sprite car1;
    public Sprite car2;
    public Sprite[] car_sprites;
    public int current_car;
    public int current_view_car;
    public TextureButton left_arrow;
    public TextureButton right_arrow;
    public PanelContainer locked;
    public TextureButton select;
    public TextureButton selected;
    private Button buyButton;
    private Label lockedLabel;
    public int buy_price;
    [Export] private PackedScene tune_component;
    [Export] public PackedScene psnotification;
    public Node2D[] tune_cards;
    public override void _Ready()
    {
        GetTree().Paused = false;
        tune_cards = new Node2D[4];
        tuningjson = File.ReadAllText(@"scripts/Tunings.json");
        if (tuningjson == "")
        {
            WritePlayerData(true);
        }
        player_json = File.ReadAllText(@"scripts/Player.json");
        money_label = (Label)GetNode("top_Hud/money");
        days_label = (Label)GetNode("top_Hud/days");
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
        money_label.Text = get_datas.money + "$";
        days_label.Text = "Days " + get_datas.Days;

        left_arrow = (TextureButton)GetNode("left_arrow");
        right_arrow = (TextureButton)GetNode("right_arrow");

        locked = (PanelContainer)GetNode("Locked");
        buyButton = GetNode("BuyButton") as Button;
        lockedLabel = GetNode("LockedLabel") as Label;


        select = (TextureButton)GetNode("select");
        selected = (TextureButton)GetNode("selected");

        car0 = (Sprite)GetNode("mid_hud/car0");
        car1 = (Sprite)GetNode("mid_hud/car1");
        car2 = (Sprite)GetNode("mid_hud/car2");

        car_sprites = new Sprite[3];
        car_sprites[0] = car0;
        car_sprites[1] = car1;
        car_sprites[2] = car2;

        current_car = get_datas.currentcar; //jelenlegi kocsi lekérdezés
        current_view_car = current_car;
        checkArrows();
        loadTuneComponents();

        //Jelenlegi autó berakása középre
        for (int i = 0; i < car_sprites.Length; i++)
        {
            car_sprites[i].SetPosition(new Vector2((car_sprites[i].Position.x - current_car * 2000), car_sprites[i].Position.y));
        }

        // Amikor legelőször be mész a garageba (amikor elindítod a játékot) akkor nem jelzi a pipát a kocsin csak ha jobra vagy balra mész
    }

    public override void _Process(float delta)
    {
        player_json = File.ReadAllText(@"scripts/Player.json");
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
        money_label.Text = get_datas.money + "$";
    }


    public void _on_left_arrow_pressed()
    {
        select.Visible = false;
        selected.Visible = false;
        locked.Visible = false;
        lockedLabel.Visible = false;
        buyButton.Visible = false;
        //Tween tween = new Tween();  ezzel lehet majd megcsinálni,hogy animáltan mozogjon ami azért valljuk be elég menő lenne! Ötletgazda:Korall, megvalósító:Korall
        right_arrow.Visible = true;
        for (int i = 0; i < car_sprites.Length; i++)
        {
            car_sprites[i].SetPosition(new Vector2((car_sprites[i].Position.x + 2000), car_sprites[i].Position.y));
        }
        current_view_car -= 1;
        checkArrows();
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
        //TUNINGOk
        string tunings = File.ReadAllText(@"scripts/Tunings.json");
        string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása
        for (int i = 0; i < tunings_split.Length; i++)
        {
            Dictionary<string, int> currentDic =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
            if (current_view_car == currentDic["id"])
            {
                if (!get_datas.UnlockedCars.Contains(current_view_car))
                {
                    locked.Visible = true;
                    lockedLabel.Visible = true;
                    buy_price = 10000 * current_view_car * current_view_car;
                    lockedLabel.Text = "Locked. " + buy_price + "$";
                    buyButton.Visible = true;
                    for (int j = 0; j < currentDic.Count; j++)
                    {
                        int index = j - 1;
                        if (currentDic.ElementAt(j).Key != "id")
                        {
                            tune_cards[index].Visible = false;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < currentDic.Count; j++)
                    {
                        int index = j - 1;
                        if (currentDic.ElementAt(j).Key != "id")
                        {
                            tune_cards[index].Visible = true;
                            tune_cards[index].Set("car_id", currentDic["id"]);
                            tune_cards[index].Set("component", currentDic.ElementAt(j).Key);
                            tune_cards[index].Set("currentLvl", currentDic.ElementAt(j).Value);
                            tune_cards[index].Position = new Vector2(j * 160 + 45, 445);
                        }
                    }

                    if (current_view_car != current_car)
                    {
                        select.Visible = true;
                    }
                    else
                    {
                        selected.Visible = true;
                    }
                }

            }
            //GD.Print(tunings_split[i]);
        }
        //

    }

    public void _on_BuyButton_pressed()
    {
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
        if (get_datas.money >= buy_price)
        {
            List<int> owned_cars = get_datas.UnlockedCars;
            owned_cars.Add(current_view_car);
            int curmoney = get_datas.money - buy_price;
            JObject options = new JObject(
            new JProperty("CurrentCar", get_datas.currentcar),
            new JProperty("Money", curmoney),
            new JProperty("UnlockedCars", owned_cars),
            new JProperty("Cars", get_datas.Cars),
            new JProperty("Days", get_datas.Days),
		    new JProperty("Maps", get_datas.Maps));
            File.WriteAllText(@"scripts/Player.json", options.ToString());
            using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                options.WriteTo(writer);
            }



            //FRISSITES//

            locked.Visible = false;
            lockedLabel.Visible = false;
            buyButton.Visible = false;


            string tunings = File.ReadAllText(@"scripts/Tunings.json");
            string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása

            string player_json2 = File.ReadAllText(@"scripts/Player.json");
            var get_datas2 = JsonConvert.DeserializeObject<ConfigBody>(player_json2);
            for (int i = 0; i < tunings_split.Length; i++)
            {
                Dictionary<string, int> currentDic =
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
                if (current_view_car == currentDic["id"])
                {
                    for (int j = 0; j < currentDic.Count; j++)
                    {
                        int index = j - 1;
                        if (currentDic.ElementAt(j).Key != "id")
                        {
                           tune_cards[index].Visible = true;
                            tune_cards[index].Set("car_id", currentDic["id"]);
                            tune_cards[index].Set("component", currentDic.ElementAt(j).Key);
                            tune_cards[index].Set("currentLvl", currentDic.ElementAt(j).Value);
                            tune_cards[index].Position = new Vector2(j * 160 + 45, 445);
                        }
                    }
                }
                //GD.Print(tunings_split[i]);
            }
            //
        }else{
            GD.Print("nocash");
            Node2D notification = (Node2D)psnotification.Instance();
            notification.Set("text","Nincs elég \n pénzed a kocsi \n megvételéhez!");
            notification.Set("color","red");
            AddChild(notification);
        }
    }

    public void _on_right_arrow_pressed()
    {
        select.Visible = false;
        selected.Visible = false;
        left_arrow.Visible = true;
        for (int i = 0; i < car_sprites.Length; i++)
        {
            car_sprites[i].SetPosition(new Vector2((car_sprites[i].Position.x - 2000), car_sprites[i].Position.y));
        }
        current_view_car += 1;
        checkArrows();
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);

        //TUNINGOk
        string tunings = File.ReadAllText(@"scripts/Tunings.json");
        string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása
        for (int i = 0; i < tunings_split.Length; i++)
        {
            Dictionary<string, int> currentDic =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
            if (current_view_car == currentDic["id"])
            {
                if (!get_datas.UnlockedCars.Contains(current_view_car))
                {
                    locked.Visible = true;
                    lockedLabel.Visible = true;
                    buy_price = 10000 * current_view_car * current_view_car;
                    lockedLabel.Text = "Locked. " + buy_price + "$";
                    buyButton.Visible = true;
                    for (int j = 0; j < currentDic.Count; j++)
                    {
                        int index = j - 1;
                        if (currentDic.ElementAt(j).Key != "id")
                        {
                            tune_cards[index].Visible = false;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < currentDic.Count; j++)
                    {
                        int index = j - 1;
                        if (currentDic.ElementAt(j).Key != "id")
                        {
                            tune_cards[index].Visible = true;
                            tune_cards[index].Set("car_id", currentDic["id"]);
                            tune_cards[index].Set("component", currentDic.ElementAt(j).Key);
                            tune_cards[index].Set("currentLvl", currentDic.ElementAt(j).Value);
                            tune_cards[index].Position = new Vector2(j * 160 + 45, 445);
                        }
                    }

                    if (current_view_car != current_car)
                    {
                        select.Visible = true;
                    }
                    else
                    {
                        selected.Visible = true;
                    }
                }

            }
            //GD.Print(tunings_split[i]);
        }
        //





    }

    public void checkArrows()
    {
        if (current_view_car == 0)
        {
            left_arrow.Visible = false;
        }
        else if (current_view_car == 2)
        {
            right_arrow.Visible = false;
        }
    }

    public void _on_Next_pressed()
    {
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void _on_Back_pressed()
    {
        GetTree().ChangeScene("res://scenes/Menu.tscn");
    }

    public void _on_select_pressed()
    {
        var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
        if (get_datas.UnlockedCars.Contains(current_view_car))
        {
            current_car = current_view_car;
            select.Visible = false;
            selected.Visible = true;
            JObject options = new JObject(
                new JProperty("CurrentCar", current_view_car),
                new JProperty("Money", get_datas.money),
                new JProperty("UnlockedCars", get_datas.UnlockedCars),
                new JProperty("Cars", get_datas.Cars),
                new JProperty("Days", get_datas.Days));
            File.WriteAllText(@"scripts/Player.json", options.ToString());
            using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                options.WriteTo(writer);
            }
        }

        //Currentcar update


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
                kocsi.Add("id", car);
                kocsi.Add("engine", 0);
                kocsi.Add("gun", 0);
                kocsi.Add("petrol", 0);
                kocsi.Add("nitro", 0);
                if (index != 0)
                {
                    File.AppendAllText(@"scripts/Tunings.json", "\n" + JsonConvert.SerializeObject(kocsi));
                }
                else
                {
                    File.AppendAllText(@"scripts/Tunings.json", JsonConvert.SerializeObject(kocsi));
                }
            }
        }
    }

    public void loadTuneComponents()
    {
        string tunings = File.ReadAllText(@"scripts/Tunings.json");
        string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása

        string player_json2 = File.ReadAllText(@"scripts/Player.json");
        var get_datas2 = JsonConvert.DeserializeObject<ConfigBody>(player_json2);
        for (int i = 0; i < tunings_split.Length; i++)
        {
            Dictionary<string, int> currentDic =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
            if (current_view_car == currentDic["id"])
            {
                for (int j = 0; j < currentDic.Count; j++)
                {
                    int index = j - 1;
                    if (currentDic.ElementAt(j).Key != "id")
                    {

                        tune_cards[index] = (Node2D)tune_component.Instance();
                        tune_cards[index].Set("car_id", currentDic["id"]);
                        tune_cards[index].Set("component", currentDic.ElementAt(j).Key);
                        tune_cards[index].Set("currentLvl", currentDic.ElementAt(j).Value);
                        tune_cards[index].Position = new Vector2(j * 160 + 45, 445);
                        AddChild(tune_cards[index]);
                    }
                }
            }
            //GD.Print(tunings_split[i]);
        }
    }
}
