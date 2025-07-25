using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Game> GamesNavigation { get; set; } = new List<Game>();
}
