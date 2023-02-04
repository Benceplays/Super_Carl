using System.Media;
using System;
using System.Collections.Generic;
public class ConfigBody
{
    public int currentcar { get; set; }
    public int money { get; set; }
    public int zombie{ get; set; }
    public List<int> UnlockedCars { get; set; }
    public List<int> Cars { get; set; }
    public int Days { get; set; }
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
