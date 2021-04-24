using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class Transaction : EntityBase
  {
    public AppUser User { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal OldAmount { get; set; }
    public decimal NewAmount { get; set; }
    public int TransactionType { get; set; }

  }
}
