namespace DoohAdManager.Models
{
    public class Screen
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public string Resolution { get; set; } = "1920x1080";
        public bool IsActive { get; set; } = true;
        
        // Navigation
        public ICollection<CampaignScreen> CampaignScreens { get; set; } = new List<CampaignScreen>();
        public ICollection<ProofOfPlay> ProofOfPlays { get; set; } = new List<ProofOfPlay>();
    }
}