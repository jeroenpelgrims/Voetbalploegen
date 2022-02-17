using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace Voetbalploegen.Scrapers
{
    public class ClubScraper
    {
        public static async Task<IEnumerable<string>> Clublinks()
        {
            var html = await new HtmlWeb().LoadFromWebAsync("https://www.voetbalkrant.com/ploegen");
            var clublinks = html
                .QuerySelectorAll("table tr td:nth-child(2) a")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(url => url != null && !url.Contains("vrouwen"))
                .Select(url => "https://www.voetbalkrant.com" + url)
                .Where(url => !url.Contains("belgie/belgie")); // Skip Rode Duivels
            return clublinks;
        }

        public string ClubUrl { get; }
        private HtmlDocument? html = null;

        public ClubScraper(string clubUrl)
        {
            ClubUrl = clubUrl;
        }

        public static string GetClubId(string clubUrl)
        {
            return clubUrl.Split("/").Last();
        }

        async Task<HtmlDocument> GetClubHtml()
        {
            if (html == null)
            {
                html = await new HtmlWeb().LoadFromWebAsync(ClubUrl);
            }

            return html;
        }

        public async Task<Club> Scrape()
        {
            var html = await GetClubHtml();
            var table = html.QuerySelector(".col-md-4 table");
            var tableParser = new TableParser(table);

            return new Club()
            {
                Id = GetClubId(ClubUrl),
                Name = GetName(html),
                Opgericht = tableParser.GetInt("Opgericht:"),
                Aansluitingsjaar = tableParser.GetInt("Aansluitingsjaar:"),
                Stadion = tableParser.GetString("Stadion:"),
                Website = tableParser.GetString("Website:"),
                Email = tableParser.GetString("E-mail:"),
                Telefoon = tableParser.GetString("Telefoon:"),
                Sponsors = tableParser.GetString("Sponsors:"),
                Bijnamen = tableParser.GetString("Bijnamen:"),
                Budget = tableParser.GetInt("Budget:")
            };
        }

        string GetName(HtmlDocument html)
        {
            return html.QuerySelector("h1").InnerText.Trim();
        }

        public async IAsyncEnumerable<Speler> Spelers()
        {
            var html = await new HtmlWeb().LoadFromWebAsync($"{ClubUrl}/spelers");
            var spelerRows = html
                .QuerySelectorAll("table tr:not(.table-secondary)")
                .TakeWhile(tr => !tr.InnerText.Contains("Trainer") && !tr.InnerText.Contains("T2") && !tr.InnerText.Contains("Manisaspor"));

            foreach (var spelerRow in spelerRows)
            {
                var spelerScraper = new SpelerScraper(GetClubId(ClubUrl), spelerRow);
                var speler = await spelerScraper.Scrape();
                yield return speler;
            }
        }

        public async Task<IEnumerable<Wedstrijd>> Wedstrijden()
        {
            var html = await GetClubHtml();
            var table = html.QuerySelector("table");
            var rows = table
                .QuerySelectorAll("tr")
                .TakeWhile(row =>
                {
                    var content = row.QuerySelector("td:nth-child(2)").InnerText;
                    return !content.Contains("/") && !content.Contains(":") && !content.Contains("Uitg");
                });
            return rows.Select(row => new WedstrijdParser(row).GetWedstrijd());
        }
    }
}
