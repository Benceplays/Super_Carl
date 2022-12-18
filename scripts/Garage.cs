using File = System.IO.File;
using System.IO;
using Godot;
using System;
public class Garage : Node2D
{
    [Export]private PackedScene carTemplate;
    private Node2D carNode;
    public string player_json;

    public override void _Ready()
    {
        player_json = File.ReadAllText(@"scripts/Player.json");
        if (player_json == "")
        {
            WritePlayerData(true);
        }
        carNode = (Node2D)carTemplate.Instance();
        AddChild(carNode);
        //todo:Olyan Car objektum osztályt csinálni amiket belehet olvasni és a property-ket betölteni.
    }

    public void _on_Next_pressed(){
        GetTree().ChangeScene("res://scenes/Game.tscn");
    }

    public void WritePlayerData(bool loadDefault)
    {
        //Amennyiben ures a json fájl =>
        if (loadDefault)
        {
            
        }
    }
}