using Microsoft.EntityFrameworkCore;
using VideotekaFm.Models;

namespace VideotekaFm.Models
{
    public class VideotekaContext : DbContext
    {
        public VideotekaContext(DbContextOptions<VideotekaContext> options) : base(options)
        {
        }

        public DbSet<Film> Filmovi { get; set; }
        public DbSet<Clan> Clanovi { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Zanr> Zanr { get; set; }
    }
}
