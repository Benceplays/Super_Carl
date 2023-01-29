using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;

public class Menu : Node2D
{
	public override void _Ready()
	{
		string text = File.ReadAllText(@"scripts/Options.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);

		// set the window mode
		switch (get_options.displaymode)
		{
			case 0:
				OS.WindowResizable = true;
				OS.WindowMaximized = false;
				OS.WindowFullscreen = false;
				break;
			case 1:
				OS.WindowFullscreen = true;
				OS.WindowMaximized = false;
				OS.WindowBorderless = false;
				break;
			case 2:
				OS.WindowMaximized = true;
				OS.WindowFullscreen = false;
				OS.WindowBorderless = false;
				break;

		}
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
