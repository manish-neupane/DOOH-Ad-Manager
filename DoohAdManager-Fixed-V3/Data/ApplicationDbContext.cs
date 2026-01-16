using Microsoft.EntityFrameworkCore;
using DoohAdManager.Models;

namespace DoohAdManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        
        public DbSet<Screen> Screens => Set<Screen>();
        public DbSet<Ad> Ads => Set<Ad>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<CampaignScreen> CampaignScreens => Set<CampaignScreen>();
        public DbSet<CampaignAd> CampaignAds => Set<CampaignAd>();
        public DbSet<ProofOfPlay> ProofOfPlays => Set<ProofOfPlay>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite primary keys for junction tables
            modelBuilder.Entity<CampaignScreen>()
                .HasKey(cs => new { cs.CampaignId, cs.ScreenId });
            
            modelBuilder.Entity<CampaignAd>()
                .HasKey(ca => new { ca.CampaignId, ca.AdId });
            
            // Relationships
            modelBuilder.Entity<CampaignScreen>()
                .HasOne(cs => cs.Campaign)
                .WithMany(c => c.CampaignScreens)
                .HasForeignKey(cs => cs.CampaignId);
            
            modelBuilder.Entity<CampaignScreen>()
                .HasOne(cs => cs.Screen)
                .WithMany(s => s.CampaignScreens)
                .HasForeignKey(cs => cs.ScreenId);
            
            modelBuilder.Entity<CampaignAd>()
                .HasOne(ca => ca.Campaign)
                .WithMany(c => c.CampaignAds)
                .HasForeignKey(ca => ca.CampaignId);
            
            modelBuilder.Entity<CampaignAd>()
                .HasOne(ca => ca.Ad)
                .WithMany(a => a.CampaignAds)
                .HasForeignKey(ca => ca.AdId);
            
            modelBuilder.Entity<ProofOfPlay>()
                .HasOne(p => p.Screen)
                .WithMany(s => s.ProofOfPlays)
                .HasForeignKey(p => p.ScreenId);
            
            modelBuilder.Entity<ProofOfPlay>()
                .HasOne(p => p.Ad)
                .WithMany(a => a.ProofOfPlays)
                .HasForeignKey(p => p.AdId);
        }
    }
}