using HYDRA.BLL.Services;
using HYDRA.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HYDRA.API.Controllers;

[ApiController]
[Route("api/suggestions")]
public class SuggestionsController : ControllerBase
{
    private readonly SuggestionService _svc;
    private readonly IConfiguration _cfg;
    private readonly HydraContext _ctx;
    public SuggestionsController(SuggestionService svc, IConfiguration cfg, HydraContext ctx)
    {
        _svc = svc; _cfg = cfg; _ctx = ctx;
    }

    public record CreateSuggestionDto(int UserId, string Title, string? PlatformText, string? Description, int? RawgId);
    public record VoteDto(int UserId);
    public record ApproveDto(int AdminUserId, string? Note, decimal DefaultPrice);

    [HttpPost]
    public async Task<ActionResult<GameSuggestion>> Create([FromBody] CreateSuggestionDto dto)
    {
        var apiKey = _cfg["Rawg:ApiKey"] ?? throw new InvalidOperationException("RAWG ApiKey chưa cấu hình.");
        var created = await _svc.CreateSuggestionAsync(dto.UserId, dto.Title, dto.PlatformText, dto.Description, dto.RawgId, apiKey);
        return Ok(new
        {
            suggestionId = created.SuggestionId,
            title = created.RawgName ?? created.Title,
            rawgName = created.RawgName,
            rawgBackgroundImg = created.RawgBackgroundImg
        });
    }

    [HttpPost("{id:int}/vote")]
    public async Task<IActionResult> Vote(int id, [FromBody] VoteDto dto)
    {
        var added = await _svc.VoteAsync(dto.UserId, id);
        return Ok(new { suggestionId = id, added, message = added ? "Voted" : "Already voted" });
    }

    [HttpPost("{id:int}/unvote")]
    public async Task<IActionResult> Unvote(int id, [FromBody] VoteDto dto)
    {
        var removed = await _svc.UnvoteAsync(dto.UserId, id);
        return Ok(new { suggestionId = id, removed, message = removed ? "Unvoted" : "Not found" });
    }

    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveDto dto)
    {
        await _svc.ApproveAsync(dto.AdminUserId, id, dto.Note, dto.DefaultPrice);
        return Ok(new { SuggestionId = id, Status = "Approved" });
    }

    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(int id, [FromBody] ApproveDto dto)
    {
        await _svc.RejectAsync(dto.AdminUserId, id, dto.Note);
        return Ok(new { SuggestionId = id, Status = "Rejected" });
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending([FromQuery] int take = 50, [FromQuery] int? userId = null)
    {
        var data = await _svc.GetTopPendingAsync(take);
        var ids = data.Select(d => d.Id).ToList();
        HashSet<int> voted = new();
        if (userId.HasValue)
        {
            var votedIds = await _svc.Context.GameSuggestionVotes
                .Where(v => v.UserId == userId.Value && ids.Contains(v.SuggestionId))
                .Select(v => v.SuggestionId)
                .ToListAsync();

            voted = votedIds.ToHashSet();
        }

        var shaped = data.Select(d => new
        {
            id = d.Id,
            title = d.Title,
            votes = d.Votes,
            rawgId = d.RawgId,
            rawgBackgroundImg = d.RawgBackgroundImg,
            rawgReleased = d.RawgReleased,
            rawgPublishers = d.RawgPublishers,
            createdAt = d.CreatedAt,
            hasVoted = userId.HasValue && voted.Contains(d.Id)
        });

        return Ok(shaped);
    }
}
