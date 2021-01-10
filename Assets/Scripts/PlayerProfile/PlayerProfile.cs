public class PlayerProfile
{
    public string Name { get; set; }     
    public int MaxLevel { get; set; }
    public int MaxScore { get; set; }

    public PlayerProfile() { }
    public PlayerProfile(string name, int maxLevel, int maxScore)
    {
        Name = name;
        MaxLevel = maxLevel;
        MaxScore = maxScore;
    }
}