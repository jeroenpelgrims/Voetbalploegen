using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Voetbalploegen
{
    public class VoetbalploegenContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Speler> Spelers { get; set; }
        public DbSet<Wedstrijd> Wedstrijden { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            options.UseSqlite($"Data Source={path + "/voetbalploegen.db"}");
            options.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Wedstrijd>().HasKey(w => new {
                w.ThuisploegId,
                w.BezoekersId,
                w.ThuisploegScore,
                w.BezoekersScore
            });
        }
    }

    public class Club
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? Opgericht { get; set; }
        public int? Aansluitingsjaar { get; set; }
        public string? Stadion { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? Telefoon { get; set; }
        public string? Sponsors { get; set; }
        public int? Budget { get; set; }
        public string? Bijnamen { get; set; }
    }

    public class Speler
    {
        public string Id { get; set; }
        public string Voornaam { get; set; }
        public string Naam { get; set; }
        public string? Nationaliteit { get; set; }
        public DateTime? Geboortedatum { get; set; }

        public string ClubId { get; set; }
        public Club Club { get; set; }

        public string? Positie { get; set; }
        public bool? Rechtsvoetig { get; set; }
        public int Wedstrijden { get; set; }
        public int AantalKeerTitularis { get; set; }
        public int AantalKeerVervangen { get; set; }
        public int AantalGespeeldeMinuten { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int AantalRodeKaarten { get; set; }
        public int AantalGeleKaarten { get; set; }
    }

    public class Wedstrijd
    {
        public string ThuisploegId { get; set; }
        public Club Thuisploeg { get; set; }
        public string BezoekersId { get; set; }
        public Club Bezoekers { get; set; }
        public int ThuisploegScore { get; set; }
        public int BezoekersScore { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Wedstrijd wedstrijd &&
                   ThuisploegId == wedstrijd.ThuisploegId &&
                   BezoekersId == wedstrijd.BezoekersId &&
                   ThuisploegScore == wedstrijd.ThuisploegScore &&
                   BezoekersScore == wedstrijd.BezoekersScore;
        }
    }
}
