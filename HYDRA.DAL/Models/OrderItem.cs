using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int GameId { get; set; }

    public decimal PriceAtTimeOfPurchase { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
