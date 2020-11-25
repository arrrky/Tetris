public class PlayerProfile
{
    private string name;

    public string Name
    {
        get { return name; }
        set
        {
            if (value == "")
            {
                name = "Unknown";
            }
            else
            {
                name = value;
            }
        }
    }        
     
    public int MaxLevel { get; set; }
    public int MaxScore { get; set; }

    public PlayerProfile()
    {
        
    }

    public PlayerProfile(string name, int maxLevel, int maxScore)
    {
        Name = name;
        MaxLevel = maxLevel;
        MaxScore = maxScore;
    }
}
