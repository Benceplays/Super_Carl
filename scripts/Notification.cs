using Godot;
using System;

public class Notification : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public Panel panel;
    public Panel line;
    public bool finished = false;
    public bool ended = false;
    public float time;
    public string text;
    public string color;
    public Label label;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        line = (Panel)GetNode("Panel/Panel2");
        panel = (Panel)GetNode("Panel");
        label = (Label)GetNode("Panel/Text");
        label.Text = text;
        switch(color){
            case "red":
            line.Modulate = Color.Color8((byte)255,(byte)0,(byte)0,(byte)255);
            break;
        }
    }

    public override void _Process(float delta)
    {
        if (!ended)
        {
            if (panel.RectPosition.y <= 80)
            {
                panel.RectPosition = new Vector2(panel.RectPosition.x, panel.RectPosition.y + 600 * delta);
            }
            else
            {
                if (!finished)
                {
                    finished = true;
                    time = 0;
                }

            }
            if (finished)
            {
                time++;
                GD.Print(time);
                if (time >= 120 && time != 2000)
                {
                    time = 2000;
                    if (panel.Modulate.a8 > 0)
                    {
                        panel.Modulate = Color.Color8((byte)panel.Modulate.r8, (byte)panel.Modulate.g8, (byte)panel.Modulate.b8, (byte)(panel.Modulate.a8 - 150 * delta));

                    }
                    else
                    {
                        panel.Modulate = Color.Color8((byte)panel.Modulate.r8, (byte)panel.Modulate.g8, (byte)panel.Modulate.b8, (byte)(0));
                        panel.QueueFree();
                        ended = true;
                    }
                }

            }
        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
