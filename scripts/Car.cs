using File = System.IO.File;
using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Car : RigidBody2D
{
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
	
    private TextureProgress petrolprogress;
	public override void _Ready()
	{
		petrolprogress = GetNode("HUD/Petrol") as TextureProgress;
        petrolprogress.Value = 100;
		kocsi = GetNode("Sprite") as Sprite;
		wheel1 = GetNode("WheelHolder/Wheel") as RigidBody2D;
		wheel2 = GetNode("WheelHolder2/Wheel") as RigidBody2D;
		wheel1.ContactMonitor = true;
		wheel1.ContactsReported = 1;
		wheel2.ContactMonitor = true;
		wheel2.ContactsReported = 1;	
		ContactMonitor = true;
		ContactsReported = 1;
		
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
			//GD.Print(tunings_split[i]);
			GD.Print(); //key alapján való lekérdezés
		}
		GD.Print(engine,gun,petrol,nitro);
		gas += petrol * 5;
		nitrosupply = nitro * 5;
	}
	public override void _PhysicsProcess(float delta)
	{
		/*if(this.Position.x > longestdistance){
			longestdistance = (int) this.Position.x;
		}*/ //Ezt a Game.cs be kene majd atrakni szerintem de idk

		if(Input.IsActionPressed("space") && nitrosupply > 0){
			nitrosupply -= 1 * delta;
			if(AppliedForce <= new Vector2(750, 0))
			{
				AddForce(new Vector2(0, kocsi.Texture.GetHeight()/4), new Vector2(25, 0));
			}
		}
		else{
			AppliedForce = new Vector2(0, 0);
		}
		GD.Print(nitrosupply);
		if(Input.IsActionPressed("forward")){
			if(gas > 0){
				if(wheel1.AngularVelocity < max_speed || wheel2.AngularVelocity < max_speed){
				wheel1.ApplyTorqueImpulse(delta * 10000);
				wheel2.ApplyTorqueImpulse(delta * 10000);	
				gas -= 1 * delta;
				}	
			}
		}
		GD.Print(riptimer);
		if (this.LinearVelocity.x < 25)
		{
    		riptimer -= delta;
		}
		else{
			riptimer = 5;
		}
		if(riptimer <= 0){
			Panel endmenu = GetNode("HUD/OutOfPetrol") as Panel;
			endmenu.Visible = true;
			GetTree().Paused = true;
		}
		if(Input.IsActionPressed("ui_right")){
			if(wheel1.GetCollidingBodies().Count == 0 && wheel2.GetCollidingBodies().Count == 0 && GetCollidingBodies().Count == 0){
				RotationDegrees += 1; 
			}
		}
		if(Input.IsActionPressed("ui_left")){
			if(wheel1.GetCollidingBodies().Count == 0 && wheel2.GetCollidingBodies().Count == 0 && GetCollidingBodies().Count == 0){
				RotationDegrees -= 1; 
			}
		}
		if(Input.IsActionPressed("ui_down")){
			if(wheel1.AngularVelocity < -max_speed || wheel2.AngularVelocity < -max_speed){
			wheel2.ApplyTorqueImpulse(delta * -10000); 
			}
		}
	}
}
