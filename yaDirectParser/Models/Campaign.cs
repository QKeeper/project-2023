using System.Collections;

namespace yaDirectParser.Models;

public class Campaign
{
    public int Id;
    public string Name;
    public List<AdGroup> AdsGroup { get; set; }
}