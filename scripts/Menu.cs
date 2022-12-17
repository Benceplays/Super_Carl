using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;

public class Menu : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    public void _on_Play_pressed()
    {
        GetTree().ChangeScene("res://scenes/Garage.tscn");
    }
    public void _on_Options_pressed() 
    { 
        GetTree().ChangeScene("res://scenes/Options.tscn");
    }
    public void _on_Exit_pressed()
    {
        GetTree().Quit();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
