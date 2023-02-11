using Godot;
using System;
using System.Collections.Generic;

public class CarTemplate : Node2D
{
    //tipusok:(1 = basecar; 2 = busz)
    public int type;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //GD.Print(type);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
