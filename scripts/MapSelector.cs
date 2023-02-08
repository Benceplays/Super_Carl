using Godot;
using System;

public class MapSelector : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Map1_pressed(){
        var game = GetNode("../Game") as Node2D;
        game.Set("map", 1);
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void _on_Map2_pressed(){
        var game = GetNode("../Game") as Node2D;
        game.Set("map", 2);
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
