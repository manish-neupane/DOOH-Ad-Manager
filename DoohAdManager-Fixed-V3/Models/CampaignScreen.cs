namespace DoohAdManager.Models
{
    public class CampaignScreen
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;
        
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = null!;
    }
}