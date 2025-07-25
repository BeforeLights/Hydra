using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<LibraryEntry> LibraryEntries { get; set; } = new List<LibraryEntry>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
