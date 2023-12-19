using System.Text.RegularExpressions;

namespace yaDirectParser.Models;

public class AdViewModel
{
    
    public int Id { get; set; }

    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public DateTime PromoDate { get; set; }

    public static List<AdViewModel> SortByDate(AdViewModel[] AdList)
    {
        return AdList
            .Select(ad=>FindDateNearWord(ad))
            .OrderByDescending(ad => ad.PromoDate).ToList();
    }
    
    public static AdViewModel FindDateNearWord(AdViewModel ad)
    {
        string filterFilePath =  Directory.GetCurrentDirectory() + "\\Filter.txt";
        var text = ad.Text;
        int index = -1;
        var wordToFind = File.ReadAllText(filterFilePath).Split(' ');
        foreach (var word in wordToFind)
        {
            if (index < 0)
                index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            else break;
        }
        if (index >= 0)
        {
            int radius = 100;
            
            int startIndex = Math.Max(0, index - radius);
            int endIndex = Math.Min(text.Length, index + wordToFind.Length + radius);
            
            string substring = text.Substring(startIndex, endIndex - startIndex);
            
            string datePattern = @"(\d{1,2}).(\d{1,2}).(\d{4})";
            
            Match match = Regex.Match(substring, datePattern);

            if (match.Success)
            {
                if (DateTime.TryParse(match.Value, out DateTime foundDate))
                {
                    ad.PromoDate = foundDate;
                    return ad;
                }
            }
        }

        return ad;
    }
}