using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Car : RigidBody2D
{
	[Export] public PackedScene psBullet;
	public int engine;
	public int nitro;
	public int petrol;
	public float gas = 15;
	public int gun;
	public int id;
	RigidBody2D wheel1;
	RigidBody2D wheel2;
	private int max_speed = 90;
	private Vector2 checkPoint;	
	private int longestdistance;
	private float riptimer = 5;
	private Sprite kocsi;
	private float nitrosupply;
	private float speed = 1;
	private float rotationcar;
	private bool onfloor;
	private RigidBody2D car;
    private TextureProgress petrolprogress;
    private Label moneylabel;
	private int money;
	private int zombiemoney;
	private Label zombielabel;
	private Label distancelabel;
	private Label totallabel;
	private int positionx;
	private StaticBody2D weapon;
	private int zombie_money = 50;
	public override void _Ready()
	{
		weapon = GetNode("Weapon") as StaticBody2D;
		kocsi = GetNode("Sprite") as Sprite;
		wheel1 = GetNode("WheelHolder/Wheel") as RigidBody2D;
		wheel2 = GetNode("WheelHolder2/Wheel") as RigidBody2D;
		wheel1.ContactMonitor = true;
		wheel1.ContactsReported = 1;
		wheel2.ContactMonitor = true;
		wheel2.ContactsReported = 1;	
		ContactMonitor = true;
		ContactsReported = 1;
		
		string text = File.ReadAllText(@"scripts/Player.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);

		string tunings = File.ReadAllText(@"scripts/Tunings.json");
		string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása
		for (int i = 0; i < tunings_split.Length; i++)
		{
			Dictionary<string, int> currentDic =
				JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
			if (id == currentDic["id"])
			{
				engine = currentDic["engine"]; // 0, 1, 2
				nitro = currentDic["nitro"]; // 0, 1, 2, 3, 
				gun = currentDic["gun"]; // 0, 1
				petrol = currentDic["petrol"]; // 0, 1, 2, 3, 4
			}
		}
		gas += petrol * 5;
		nitrosupply = nitro * 5;
		speed = engine + 1;
		if(gun == 1){
			weapon.Visible = true;
		}
	}
	public void End(){
        string textplayer = File.ReadAllText(@"scripts/Player.json");
		var get_optionsplayer = JsonConvert.DeserializeObject<ConfigBody>(textplayer);
		zombiemoney = zombie_money * get_optionsplayer.zombie;
		int plusmoney = money + zombiemoney;

        JObject options = new JObject(
            new JProperty("CurrentCar", get_optionsplayer.currentcar),
            new JProperty("Money", get_optionsplayer.money + plusmoney),
			new JProperty("Zombie", get_optionsplayer.zombie),
            new JProperty("UnlockedCars", get_optionsplayer.UnlockedCars),
            new JProperty("Cars", get_optionsplayer.Cars),
            new JProperty("Days", get_optionsplayer.Days));
        File.WriteAllText(@"scripts/Player.json", options.ToString());
        using (StreamWriter file = File.CreateText(@"scripts/Player.json"))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            options.WriteTo(writer);
        }
		
        zombielabel = GetNode("HUD/OutOfPetrol/Zombie") as Label;
        distancelabel = GetNode("HUD/OutOfPetrol/Distance") as Label;
        totallabel = GetNode("HUD/OutOfPetrol/TotalLabel") as Label;

		zombielabel.Text = $"Zombie Hit: ${zombiemoney}";
		distancelabel.Text = $"Distance: ${money}";
		totallabel.Text = $"Total Money: ${plusmoney}";

	}

	public override void _PhysicsProcess(float delta)
	{
        moneylabel = GetNode("HUD/money") as Label;
		car = GetNode("/root/Game/Car") as RigidBody2D;
		if(money < positionx){
			money = positionx;
       		moneylabel.Text = "Money: " + money;
		}else{
			money = (int) car.Position.x / 100;
        	moneylabel.Text = "Money: " + money;
		}
		// buggos a kiíratása a moneynak
		
		/*if(this.Position.x > longestdistance){
			longestdistance = (int) this.Position.x;
		}*/ //Ezt a Game.cs be kene majd atrakni szerintem de idk

		/*if(Input.IsActionPressed("space") && nitrosupply > 0){
			nitrosupply -= 1 * delta;
			if(AppliedForce <= new Vector2(1500, 0))
			{
				ApplyImpulse(new Vector2(kocsi.Texture.GetWidth()/4, kocsi.Texture.GetHeight()/4), new Vector2(50, 0));
			}
		}
		else{
			//impulse = new Vector2(0, 0);
		}*/
		
		if(Input.IsActionPressed("forward")){
			if(gas > 0){
				if((wheel1.AngularVelocity < max_speed || wheel2.AngularVelocity < max_speed) && onfloor == true){
				wheel1.ApplyTorqueImpulse(delta * 10000 * speed);
				wheel2.ApplyTorqueImpulse(delta * 10000 * speed);	
				gas -= 1 * delta;
				}	
			}
		}
		petrolprogress = GetNode("HUD/Petrol") as TextureProgress;
        petrolprogress.Value = gas;
		//GD.Print(riptimer);
		if (this.LinearVelocity.x < 25)
		{
    		riptimer -= delta;
			if(positionx <= (int) car.Position.x / 100){
				positionx = (int) car.Position.x / 100;
			}
		}
		else{
			riptimer = 5;
		}
		if(riptimer <= 0){
			Panel endmenu = GetNode("HUD/OutOfPetrol") as Panel;
			endmenu.Visible = true;
			GetTree().Paused = true;
			money = positionx;
			End();
		}
		if(wheel1.GetCollidingBodies().Count == 0 && wheel2.GetCollidingBodies().Count == 0 && GetCollidingBodies().Count == 0){
			onfloor = false;
			if(Input.IsActionPressed("ui_right")){
				RotationDegrees += 1; 
			}
			if(Input.IsActionPressed("ui_left")){
				RotationDegrees -= 1; 
			}
		}
		else{
			onfloor = true;
			rotationcar = Rotation;
		}
		
		/*if(Input.IsActionPressed("ui_down")){
			if(wheel1.AngularVelocity < -max_speed || wheel2.AngularVelocity < -max_speed){
			wheel2.ApplyTorqueImpulse(delta * -10000); 
			}
		}*/ 
	}

	private void _on_AreaWeapon_body_entered(KinematicBody2D enemy){
		if(enemy.IsInGroup("zombie") && weapon.Visible == true){
			
			var gamenode = GetNode("../") as Node2D;
            KinematicBody2D bullet = (KinematicBody2D)psBullet.Instance();
			bullet.Set("enemyposition", enemy.Position);
			bullet.Position = Position;
			gamenode.CallDeferred("add_child", bullet);
		}
	}
}
