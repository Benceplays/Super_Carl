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
}
