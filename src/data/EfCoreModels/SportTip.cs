using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class SportTip
  {
    public Sport Sport { get; set; }
    public int SportId { get; set; }
    public Tip Tip { get; set; }
    public int TipId { get; set; }
  }
}
