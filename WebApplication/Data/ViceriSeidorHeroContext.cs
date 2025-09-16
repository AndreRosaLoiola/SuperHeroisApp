using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using ViceriSeidorHero.Models;

namespace ViceriSeidorHero.Data
{
    public class ViceriSeidorHeroContext : DbContext
    {
        public ViceriSeidorHeroContext(DbContextOptions<ViceriSeidorHeroContext> options)
            : base(options) { }

        public DbSet<Heroi> Herois { get; set; }
        public DbSet<Superpoderes> Superpoderes { get; set; }
        public DbSet<HeroiSuperpoder> HeroisSuperpoderes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeroiSuperpoder>()
                .HasKey(hs => new { hs.HeroiId, hs.SuperpoderId });

            modelBuilder.Entity<HeroiSuperpoder>()
                .HasOne(hs => hs.Heroi)
                .WithMany(h => h.HeroisSuperpoderes)
                .HasForeignKey(hs => hs.HeroiId);

            modelBuilder.Entity<HeroiSuperpoder>()
                .HasOne(hs => hs.Superpoder)
                .WithMany(s => s.HeroisSuperpoderes)
                .HasForeignKey(hs => hs.SuperpoderId);

            modelBuilder.Entity<Heroi>()
                .HasIndex(h => h.NomeHeroi)
                .IsUnique();
        }
    }
}
