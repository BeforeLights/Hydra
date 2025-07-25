using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class GameImage
{
    public int ImageId { get; set; }

    public int GameId { get; set; }

    public string ImagePath { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Game Game { get; set; } = null!;
}
