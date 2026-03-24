public interface IAuctionService
{
    Task<List<Auction>> GetAllAuctionsAsync();
    Task<Auction?> GetAuctionAsync(int id);
    Task<bool> PlaceBidAsync(int id, decimal amount);
}