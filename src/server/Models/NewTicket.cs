using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
  public class NewTicket
  {
    public List<int> PairTips { get; set; }
    public int SuperTip { get; set; }
    public decimal MoneyAmount { get; set; }
  }
}
