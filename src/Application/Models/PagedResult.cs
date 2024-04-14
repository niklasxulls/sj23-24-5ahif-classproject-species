using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Models;
public class PagedResult<T>
{
    public ICollection<T> Items { get; set; }
    public Paging Paging { get; set; }
}
