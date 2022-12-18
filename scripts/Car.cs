using Godot;
using System;

public class Car : RigidBody2D
{
	public int enginelvl;
	public int wheellvl;
	public int petrollvl;
	public int gunlvl;
	public int id;
	RigidBody2D wheel1;
	RigidBody2D wheel2;
	private int max_speed = 90;
	private Vector2 checkPoint;	
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
		GD.Print(id);
	}
	public override void _PhysicsProcess(float delta)
	{
		if(Input.IsActionPressed("forward")){
			if(wheel1.AngularVelocity < max_speed || wheel2.AngularVelocity < max_speed){
			wheel1.ApplyTorqueImpulse(delta * 10000);
			wheel2.ApplyTorqueImpulse(delta * 10000);	
			}
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
