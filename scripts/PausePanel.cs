using Godot;
using System;

public class PausePanel : Panel
{
    public bool pauseison = false;
    public PausePanel pausepanel;
    
    public override void _Ready()
    {
        pausepanel = GetNode("/root/Game/Car/HUD/PausePanel") as PausePanel;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            if (!pauseison)
            {
                pausepanel.Visible = true;
                pauseison = true;
                GetTree().Paused = true;
            }
            else
            {
                pausepanel.Visible = false;
                pauseison = false;
                GetTree().Paused = false;
            }
        }
    }

    public void _on_ReturnButton_pressed()
    {
        pausepanel.Visible = false;
        pauseison = false;
        GetTree().Paused = false;
    }

    public void _on_BackToMenuButton_pressed()
    {
        GetTree().ChangeScene("res://scenes/Menu.tscn");
        GetTree().Paused = false; 
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
