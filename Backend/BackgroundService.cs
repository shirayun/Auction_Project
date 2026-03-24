public class AuctionBackgroundService : BackgroundService
{
    private readonly IAuctionService _auctionService;

    public AuctionBackgroundService(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            List<Auction> auctions = await _auctionService.GetAllAuctionsAsync();

            foreach (var auction in auctions)
            {
                if (auction.Status == "Open" && auction.EndTime <= now)
                    auction.Status = "Closed";
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}