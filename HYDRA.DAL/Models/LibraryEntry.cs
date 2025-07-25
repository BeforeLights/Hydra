using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class LibraryEntry
{
    public int LibraryEntryId { get; set; }

    public int UserId { get; set; }

    public int GameId { get; set; }

    public int StatusId { get; set; }

    public int? UserRating { get; set; }

    public decimal? HoursPlayed { get; set; }

    public string? Notes { get; set; }

    public DateTime DateAdded { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
