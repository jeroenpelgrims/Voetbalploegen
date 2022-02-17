using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace Voetbalploegen.Scrapers
{
    public class WedstrijdParser
    {
        private HtmlNode row;

        public WedstrijdParser(HtmlNode row)
        {
            this.row = row;
        }

        public HtmlNode GetCellA(int cellIndex)
        {
            return row.QuerySelector($"td:nth-child({cellIndex+1}) a");
        }

        public string GetTeamId(int cellIndex)
        {
            var a = GetCellA(cellIndex);
            var href = a.GetAttributeValue("href", null);
            return ClubScraper.GetClubId(href);
        }

        public (int, int) GetScores()
        {
            var scores = GetCellA(1).InnerText.Trim().Trim('P').Split("-");
            var ints = scores.Select(int.Parse);
            return (ints.First(), ints.Last());
        }

        internal Wedstrijd GetWedstrijd()
        {
            var (thuis, bezoekers) = GetScores();

            return new Wedstrijd()
            {
                ThuisploegId = GetTeamId(0),
                BezoekersId = GetTeamId(2),
                ThuisploegScore = thuis,
                BezoekersScore = bezoekers
            };
        }
    }
}
