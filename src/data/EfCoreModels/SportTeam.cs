using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class SportTeam : EntityBase
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public Sport Sport { get; set; }
    public int SportId { get; set; }
    public ICollection<SportTip> SportTips { get; set; }
  }
}
