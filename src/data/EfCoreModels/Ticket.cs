﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class Ticket : EntityBase
  {
    public int Status { get; set; }
    public int PrizeAmount { get; set; }
    public int Coefficient { get; set; }
    public decimal ManipulativeCosts { get; set; }
    public Pair SpecialOfferPair { get; set; }
    public int? SpecialOfferPairId { get; set; }
    public ICollection<TicketPairTip> TicketPairTips { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
  }
}
