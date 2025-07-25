using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
