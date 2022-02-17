using Voetbalploegen;
using Voetbalploegen.Scrapers;

var database = new VoetbalploegenContext();
database.Database.EnsureCreated();

var clubLinks = await ClubScraper.Clublinks();
var clubScrapers = clubLinks.Select(url => new ClubScraper(url)).ToList();//.Skip(9).Take(1);

// Add all clubs
foreach (var clubScraper in clubScrapers)
{    
    var club = await clubScraper.Scrape();
    if (!database.Clubs.Contains(club))
    {
        database.Clubs.Add(club);
        Console.WriteLine($"Add {club.Name}");
    } else
    {
        Console.WriteLine($"Skip {club.Name}");
    }
    database.SaveChanges();
}

// Add all wedstrijden
foreach (var clubScraper in clubScrapers)
{
    var clubIds = database.Clubs.Select(club => club.Id).ToList();
    var wedstrijden = await clubScraper.Wedstrijden();
    var wedstrijdenKnownClubs = wedstrijden
        .Where(w => clubIds.Contains(w.ThuisploegId) && clubIds.Contains(w.BezoekersId));
    var missingWedstrijden = wedstrijdenKnownClubs
        .Where(wedstrijd => !database.Wedstrijden.Contains(wedstrijd)).ToList();
    database.AddRange(missingWedstrijden);
    Console.WriteLine($"Added {missingWedstrijden.Count} missing wedstrijden");
    database.SaveChanges();
}

// Add all spelers
foreach (var clubScraper in clubScrapers)
{
    await foreach (var speler in clubScraper.Spelers())
    {
        if (!database.Spelers.Contains(speler))
        {
            database.Spelers.Add(speler);
            Console.WriteLine($"Added speler {speler.Voornaam} {speler.Naam}");
        }
        else
        {
            Console.WriteLine($"Skipped speler {speler.Voornaam} {speler.Naam}");
        }
    }
    database.SaveChanges();
}