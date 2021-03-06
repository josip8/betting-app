using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
  public enum TicketStatus
  {
    Pending,
    Success,
    Fail
  }

  public enum PairStatus
  {
    Pending,
    Success,
    Fail
  }

  public enum TransactionType
  {
    Payment,
    Withdraw,
    TicketPayment,
    TicketWin
  }
}
