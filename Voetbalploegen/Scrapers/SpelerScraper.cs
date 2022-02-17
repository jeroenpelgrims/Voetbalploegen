using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace Voetbalploegen.Scrapers
{
    public class SpelerScraper
    {
        public HtmlNode SpelerRow { get; }
        public string ClubId { get; set; }
        public string SpelerUrl {
            get {
                var a = SpelerRow.QuerySelector("a");
                var href = a.GetAttributeValue("href", null);
                return $"https://www.voetbalkrant.com{href}";
            }
        }
        public string Id {
            get {
                return SpelerUrl.Split("/").Last();
            }
        }

        public SpelerScraper(string clubId, HtmlNode spelerRow)
        {
            SpelerRow = spelerRow;
            ClubId = clubId;
        }

        public async Task<Speler> Scrape()
        {
            var html = await new HtmlWeb().LoadFromWebAsync(SpelerUrl);
            var table = GetTable(html);
            var tableParser = new TableParser(table);
            
            var geboorteDatumString = tableParser.GetString("Geboortedatum:");
            DateTime? geboorteDatum = geboorteDatumString != null ? DateTime.ParseExact(geboorteDatumString, "dd/MM/yyyy", null) : null;

            var rechtsvoetigTekst = tableParser.GetString("Voet:")?.Trim();
            var rechtsvoetig = rechtsvoetigTekst != null ? rechtsvoetigTekst == "Rechts" : true;

            return new Speler() {
                Id = Id,
                Voornaam = tableParser.GetString("Voornaam:"),
                Naam = tableParser.GetString("Naam:"),
                Nationaliteit = tableParser.GetString("Nationaliteit:"),
                Geboortedatum = geboorteDatum,
                ClubId = ClubId,
                Positie = tableParser.GetString("Positie:")?.Trim(),
                Rechtsvoetig = rechtsvoetig,
                Wedstrijden = GetIntFromSpelerRow(2),
                AantalKeerTitularis = GetIntFromSpelerRow(3),
                AantalKeerVervangen = GetIntFromSpelerRow(4),
                AantalGespeeldeMinuten = GetIntFromSpelerRow(5),
                Goals = GetIntFromSpelerRow(6),
                Assists = GetIntFromSpelerRow(7),
                AantalRodeKaarten = GetIntFromSpelerRow(8),
                AantalGeleKaarten = GetIntFromSpelerRow(9),
            };
        }

        HtmlNode GetTable(HtmlDocument html)
        {
            var divs = html.QuerySelectorAll("div.title-underline");
            var infoDivs = divs.Where(div => div.InnerText.Trim() == "Info");
            var infoTitle = infoDivs.First();
            return infoTitle.NextSiblingElement();
            // return html.QuerySelector(".col-md-4 table") ?? html.QuerySelector("table");
        }

        int GetIntFromSpelerRow(int index)
        {
            var td = SpelerRow.QuerySelector($"td:nth-child({index + 1})");
            var content = td?.InnerText;
            return int.Parse(content); // In principe heeft elke speler voor elk veld een waarde.
        }
    }
}
