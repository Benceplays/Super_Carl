using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
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
	public Button left_arrow;
	public Button right_arrow;
	public PanelContainer locked;
	public TextureButton select;
	public TextureButton selected;
	public override void _Ready()
	{
		GetTree().Paused = false;
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

		left_arrow = (Button)GetNode("left_arrow");
		right_arrow = (Button)GetNode("right_arrow");

		locked = (PanelContainer)GetNode("Locked");

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

		//Jelenlegi autó berakása középre
		for (int i = 0; i < car_sprites.Length; i++)
		{
			car_sprites[i].SetPosition(new Vector2((car_sprites[i].Position.x - current_car*2000), car_sprites[i].Position.y));
		}
	}

	public override void _Process(float delta)
	{
	}
	
	
	public void _on_left_arrow_pressed()
	{
		select.Visible = false;
		selected.Visible = false;
		locked.Visible = false;
		//Tween tween = new Tween();  ezzel lehet majd megcsinálni,hogy animáltan mozogjon ami azért valljuk be elég menő lenne! Ötletgazda:Korall, megvalósító:Korall
		right_arrow.Visible = true;
		for (int i = 0; i < car_sprites.Length; i++)
		{
			car_sprites[i].SetPosition(new Vector2((car_sprites[i].Position.x + 2000), car_sprites[i].Position.y));
		}
		current_view_car -= 1;
		checkArrows();
		var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
		if (!get_datas.UnlockedCars.Contains(current_view_car))
		{
			locked.Visible = true;
		}
		else
		{
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
		if (!get_datas.UnlockedCars.Contains(current_view_car))
		{
			locked.Visible = true;
		}else
		{
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

	public void checkArrows()
	{
		if (current_view_car == 0)
		{
			left_arrow.Visible = false;
		}else if (current_view_car == 2)
		{
			GD.PrintRaw("gecikolar");
			right_arrow.Visible = false;
		}
	}

	public void _on_Next_pressed(){
		GetTree().ChangeScene("res://scenes/Game.tscn");
	}

	public void _on_Back_pressed()
	{
		
	}

	public void _on_select_pressed()
	{
		var get_datas = JsonConvert.DeserializeObject<ConfigBody>(player_json);
		if (get_datas.UnlockedCars.Contains(current_view_car))
		{
			current_car = current_view_car;
			select.Visible = false;
			selected.Visible = true;
			string textplayer = File.ReadAllText(@"scripts/Player.json");
			var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
            JObject options = new JObject(
                new JProperty("CurrentCar", current_view_car),
                new JProperty("Money", get_optionsplayer.money),
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
				kocsi.Add("id",car);
				kocsi.Add("engine",0);
				kocsi.Add("gun",0);
				kocsi.Add("petrol",0);
				kocsi.Add("nitro", 0);
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
