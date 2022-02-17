using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace Voetbalploegen.Scrapers
{
    public class TableParser
    {
        public HtmlNode Table { get; set; }
        public TableParser(HtmlNode table)
        {
            Table = table;
        }

        public string? GetString(string key)
        {
            var targetRow = Table
                .QuerySelectorAll("tr")
                .Where(tr => tr.InnerText.Contains(key))
                .FirstOrDefault();
            var targetCell = targetRow?.QuerySelector("td:nth-child(2)");
            return targetCell?.InnerText;
        }

        public int? GetInt(string key)
        {
            var text = GetString(key);

            try
            {
                var textCleaned = text?
                    .Replace(".", "")
                    .Replace(" ", "")
                    .Replace("€", "");

                if (textCleaned == null || textCleaned.Length == 0)
                {
                    return null;
                }

                return int.Parse(textCleaned);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
