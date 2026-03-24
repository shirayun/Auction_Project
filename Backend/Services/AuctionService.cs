using Microsoft.EntityFrameworkCore;

public class AuctionService : IAuctionService
{
    private readonly AuctionDbContext _context;
    private static readonly object _lock = new object();

    public AuctionService(AuctionDbContext context)
    {
        _context = context;
    }

    public async Task<List<Auction>> GetAllAuctionsAsync()
    {
        return await _context.Auctions.ToListAsync();
    }

    public async Task<Auction?> GetAuctionAsync(int id)
    {
        return await _context.Auctions.FindAsync(id);
    }

    public async Task<bool> PlaceBidAsync(int id, decimal amount)
    {
        lock (_lock)
        {   
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null || amount <= auction.CurrentPrice)
                return false;

            auction.CurrentPrice = (int)amount;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}