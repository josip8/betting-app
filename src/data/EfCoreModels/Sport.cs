using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class Sport : EntityBase
  {
    public string Name { get; set; }
    public ICollection<SportTeam> SportTeams { get; set; }
  }
}
