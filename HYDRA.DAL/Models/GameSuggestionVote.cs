using System;

namespace HYDRA.DAL.Models;

public partial class GameSuggestionVote
{
    public int SuggestionId { get; set; }
    public int UserId { get; set; }
    public DateTime VotedAt { get; set; }

    public virtual GameSuggestion Suggestion { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
