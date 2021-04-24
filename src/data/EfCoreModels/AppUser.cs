using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
  
namespace data.EfCoreModels
{
    public class AppUser: IdentityUser
    {
      public decimal WalletAmount { get; set; }
      public ICollection<Transaction> Transactions { get; set; }
      public ICollection<Ticket> Tickets { get; set; }
    }
}