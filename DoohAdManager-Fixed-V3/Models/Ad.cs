using System.Text.Json.Serialization;

namespace DoohAdManager.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int DurationSeconds { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public string? MediaType { get; set; }
        
        // Navigation properties
        [JsonIgnore]
        public ICollection<CampaignAd> CampaignAds { get; set; } = new List<CampaignAd>();
        
        [JsonIgnore]
        public ICollection<ProofOfPlay> ProofOfPlays { get; set; } = new List<ProofOfPlay>();
    }
}
