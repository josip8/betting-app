using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class TicketPairTip
  {
    public Ticket Ticket { get; set; }
    public int TicketId { get; set; }
    public PairTip PairTip { get; set; }
    public int PairTipId { get; set; }
  }
}
