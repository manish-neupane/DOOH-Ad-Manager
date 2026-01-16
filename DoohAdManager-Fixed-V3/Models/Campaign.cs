namespace DoohAdManager.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        // Navigation
        public ICollection<CampaignScreen> CampaignScreens { get; set; } = new List<CampaignScreen>();
        public ICollection<CampaignAd> CampaignAds { get; set; } = new List<CampaignAd>();
    }
}