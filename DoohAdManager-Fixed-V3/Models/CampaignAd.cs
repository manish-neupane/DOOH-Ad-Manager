namespace DoohAdManager.Models
{
    public class CampaignAd
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;
        
        public int AdId { get; set; }
        public Ad Ad { get; set; } = null!;
        
        public int PlayOrder { get; set; } //  sequence number
    }
}

