using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int GameId { get; set; }

    public DateTime DateAdded { get; set; }

    public virtual ShoppingCart Cart { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}
