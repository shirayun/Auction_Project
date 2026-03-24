using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly IHubContext<AuctionHub> _hub;

    public AuctionController(IAuctionService auctionService, IHubContext<AuctionHub> hub)
    {
        _auctionService = auctionService;
        _hub = hub;
    }

    [HttpGet]
    public async Task<IEnumerable<Auction>> GetAllAuctions()
    {
        return await _auctionService.GetAllAuctionsAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuction(int id)
    {
        var auction = await _auctionService.GetAuctionAsync(id);
        if (auction == null) return NotFound();
        return Ok(auction);
    }

    [HttpPost("{id}/bid")]
    public async Task<IActionResult> PlaceBid(int id, [FromBody] Bid bid)
    {
        bool success = await _auctionService.PlaceBidAsync(id, bid.Amount);
        if (!success) return BadRequest("Bid too low or auction closed.");

        await _hub.Clients.All.SendAsync("ReceiveBidUpdate", new
        {
            AuctionId = id,
            Price = bid.Amount
        });
        return Ok();
    }
}