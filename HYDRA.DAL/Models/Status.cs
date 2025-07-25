using System;
using System.Collections.Generic;

namespace HYDRA.DAL.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<LibraryEntry> LibraryEntries { get; set; } = new List<LibraryEntry>();
}
