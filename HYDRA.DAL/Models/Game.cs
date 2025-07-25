using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? CoverArtPath { get; set; }

    public int? PublisherId { get; set; }

    public decimal Price { get; set; }

    public bool IsForSale { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<GameImage> GameImages { get; set; } = new List<GameImage>();

    public virtual ICollection<LibraryEntry> LibraryEntries { get; set; } = new List<LibraryEntry>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Company? Publisher { get; set; }

    public virtual ICollection<Company> Developers { get; set; } = new List<Company>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
