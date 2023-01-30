using Godot;
using System;
using File = System.IO.File;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class TuneComponent : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public int car_id;
    public string component;
    public ProgressBar lvlProgress;
    public Label name;
    public int currentLvl;
    public int maxLvl; //itt a maxlvl számolása nem nullától van, tehát ha 0;1;2-es szintek vannak akkor a maximalis szint 3
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lvlProgress = (ProgressBar)GetNode("Panel/lvlProgress");
        lvlProgress.Value = currentLvl;
        name = (Label)GetNode("Panel/name");
        name.Text = component.ToUpper();
        switch (component)
        {
            case "engine":
                maxLvl = 2;
                break;
            case "nitro":
                maxLvl = 3;
                break;
            case "gun":
                maxLvl = 1;
                break;
            case "petrol":
                maxLvl = 4;
                break;
        }
        lvlProgress.MaxValue = maxLvl;
    }

    public void _on_buyButton_pressed()
    {
        if (currentLvl < maxLvl)
        {
            currentLvl++;
            refreshProgressBar();
        }
    }

    public void refreshProgressBar()
    {
        lvlProgress.Value = currentLvl;
    }

    public override void _Process(float delta)
    {
        refreshProgressBar();
    }

    public void writeToJSON()
    {
        string tunings = File.ReadAllText(@"scripts/Tunings.json");
        string[] tunings_split = tunings.Split("\n"); //Sorokra való felosztása
        /*
        for (int i = 0; i < tunings_split.Length; i++)
        {
            Dictionary<string, int> currentDic =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(tunings_split[i]);
            if (car_id == currentDic["id"])
            {
                engine = currentDic["engine"]; // 0, 1, 2
                nitro = currentDic["nitro"]; // 0, 1, 2, 3, 
                gun = currentDic["gun"]; // 0, 1
                petrol = currentDic["petrol"]; // 0, 1, 2, 3, 4
            }
            //GD.Print(tunings_split[i]);
            GD.Print(); //key alapján való lekérdezés
        }
        */ //IRAS JSONBA AMIKOR A JATEKOS MEGVESZ EGY TUNING KOMPONENSET!?!!?!?!!?

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
