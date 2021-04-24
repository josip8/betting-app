using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class TicketPair
  {
    public Ticket Ticket { get; set; }
    public int TicketId { get; set; }
    public Pair Pair { get; set; }
    public int PairId { get; set; }
  }
}
