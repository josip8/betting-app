using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
  
namespace data.EfCoreModels
{
    public class AppUser: IdentityUser
    {
      public decimal Amount { get; set; }
    }
}