using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
