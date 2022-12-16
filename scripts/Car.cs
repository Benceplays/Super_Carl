using Godot;
using System;

public class Car : RigidBody2D
{
	RigidBody2D wheel1;
	RigidBody2D wheel2;
	public override void _Ready()
	{
		wheel1 = GetNode("WheelHolder/Wheel") as RigidBody2D;
		wheel2 = GetNode("WheelHolder2/Wheel") as RigidBody2D;
	}
	public override void _PhysicsProcess(float delta)
	{
		if(Input.IsActionPressed("forward")){
			wheel1.ApplyTorqueImpulse(delta * 60000 * 60);
			wheel2.ApplyTorqueImpulse(delta * 60000 * 60);
		}
		if(Input.IsActionPressed("ui_right")){
			RotationDegrees += 0.5f; 
		}
		if(Input.IsActionPressed("ui_left")){
			RotationDegrees -= 0.5f; 
		}
		if(Input.IsActionPressed("ui_down")){
			wheel2.ApplyTorqueImpulse(delta * -60000 * 60); 
			
		}
	}
}
