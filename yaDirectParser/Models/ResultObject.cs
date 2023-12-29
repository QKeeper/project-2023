namespace yaDirectParser.Models;

public class ResultObject
{
    public List<Campaign> Campaigns { get; set; }
    public List<AdGroup> AdGroups { get; set; }
    public List<Ad>? Ads { get; set; }
}