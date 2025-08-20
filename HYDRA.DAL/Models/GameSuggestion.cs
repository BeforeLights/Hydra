using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class GameSuggestion
{
    public int SuggestionId { get; set; }
    public string Title { get; set; } = null!;
    public string? PlatformText { get; set; }
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte Status { get; set; } = 0; // 0=Pending,1=Approved,2=Rejected
    public int? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedNote { get; set; }

    // RAWG snapshot
    public int? RawgId { get; set; }
    public string? RawgSlug { get; set; }
    public string? RawgName { get; set; }
    public DateOnly? RawgReleased { get; set; }
    public int? RawgMetacritic { get; set; }
    public string? RawgBackgroundImg { get; set; }
    public string? RawgPlatforms { get; set; }
    public string? RawgGenres { get; set; }

    public string? RawgPublishers { get; set; }

  

    public virtual User CreatedByUser { get; set; } = null!;
    public virtual User? ApprovedByUser { get; set; }
    public virtual ICollection<GameSuggestionVote> Votes { get; set; } = new List<GameSuggestionVote>();
}

public enum SuggestionStatus : byte
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}
