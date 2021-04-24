using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class PairTip
  {
    public Pair Pair { get; set; }
    public int PairId { get; set; }
    public Tip Tip { get; set; }
    public int TipId { get; set; }
  }
}
