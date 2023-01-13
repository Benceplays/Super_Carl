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
	public int wheel;
	public int petrol;
	public float gas;
	public int gun;
	public int id;
	RigidBody2D wheel1;
	RigidBody2D wheel2;
	private int max_speed = 90;
	private Vector2 checkPoint;	
	private int longestdistance;
	private float riptimer = 5;
	public override void _Ready()
	{
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
				wheel = currentDic["wheel"]; // 0, 1 // Ebből majd nitro lesz talán
				gun = currentDic["gun"]; // 0, 1 
				petrol = currentDic["petrol"]; // 0, 1, 2, 3, 4
			}
			//GD.Print(tunings_split[i]);
			GD.Print(); //key alapján való lekérdezés
		}
		GD.Print(engine,gun,petrol,wheel);
		switch(petrol){
			case 0:
				gas = 15;
				break;
			case 1:
				gas = 20;
				break;
			case 2:
				gas = 25;
				break;
			case 3:
				gas = 30;
				break;
			case 4:
				gas = 35;
				break;
			default:
				break;
		}
	}
	public override void _PhysicsProcess(float delta)
	{
		/*if(this.Position.x > longestdistance){
			longestdistance = (int) this.Position.x;
		}*/ //Ezt a Game.cs be kene majd atrakni szerintem de idk

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
