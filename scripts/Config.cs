using System.Media;
public class ConfigBody
{
    private static int currentcarvalue;
    public int currentcar
    {
        get { return currentcarvalue; }
        set { currentcarvalue = value; }
    }
    private static int moneyvalue;
    public int money
    {
        get { return moneyvalue; }
        set { moneyvalue = value; }
    }
    
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
