using System.Media;
using System;
using System.Collections.Generic;
public class ConfigBody
{
    public int currentcar { get; set; }
    public int money { get; set; }
    public List<int> UnlockedCars { get; set; }
    //Options//
    public int musicvolume { get; set; }
    public int soundeffectvolume { get; set; }
    public int uivolume { get ; set; }
    public bool fpsison { get; set; }
    public int fps{ get; set; }
    public bool vsync { get; set; }
    public int displaymode { get; set; }
    //Options END//
    
}
