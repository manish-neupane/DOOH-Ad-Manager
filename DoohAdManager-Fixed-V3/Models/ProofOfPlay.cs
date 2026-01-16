namespace DoohAdManager.Models
{
    public class ProofOfPlay
    {
        public int Id { get; set; }
        
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = null!;
        
        public int AdId { get; set; }
        public Ad Ad { get; set; } = null!;
        
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}