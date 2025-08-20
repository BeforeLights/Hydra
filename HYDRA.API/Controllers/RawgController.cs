using HYDRA.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace HYDRA.API.Controllers;

[ApiController]
[Route("api/rawg")]
public class RawgController : ControllerBase
{
    private readonly SuggestionService _svc;
    private readonly IConfiguration _cfg;
    public RawgController(SuggestionService svc, IConfiguration cfg)
    {
        _svc = svc; _cfg = cfg;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int take = 10)
    {
        var key = _cfg["Rawg:ApiKey"] ?? throw new InvalidOperationException("RAWG ApiKey chưa cấu hình.");
        var list = await _svc.SearchRawgAsync(q, key, take);
        return Ok(list);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var key = _cfg["Rawg:ApiKey"] ?? throw new InvalidOperationException("RAWG ApiKey chưa cấu hình.");
        var dto = await _svc.GetRawgDetailForClientAsync(id, key);
        if (dto == null) return NotFound();
        return Ok(dto);  // Trả về chi tiết game từ RAWG
    }
    [HttpGet("by-name")]
    public async Task<IActionResult> GetDetailByName([FromQuery] string q)
    {
        var key = _cfg["Rawg:ApiKey"] ?? throw new InvalidOperationException("RAWG ApiKey chưa cấu hình.");
        var dto = await _svc.GetRawgDetailByNameForClientAsync(q, key);
        if (dto == null) return NotFound();
        return Ok(dto);
    }
    [HttpGet("top")]
    public async Task<IActionResult> Top([FromQuery] int take = 100)
    {
        var data = await _svc.GetTopPendingAsync(take);

        // shape phẳng để client bind dễ dàng
        var shaped = data.Select(x => new
        {
            suggestionId = x.Id,
            title = x.Title,
            voteCount = x.Votes,
            rawgBackgroundImg = x.RawgBackgroundImg,
            rawgId = x.RawgId,
            createdAt = x.CreatedAt,
            rawgReleased = x.RawgReleased,
            rawgPublishers = x.RawgPublishers
        });

        return Ok(shaped);
    }
    [HttpGet("pending")]
    public Task<IActionResult> Pending([FromQuery] int take = 100) => Top(take);
}
