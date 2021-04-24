using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class Pair : EntityBase
  {
    public bool SpecialOffer { get; set; }
    public float SpecialOfferModifier { get; set; }
    public SportTeam HomeTeam { get; set; }
    public int HomeTeamId { get; set; }
    public SportTeam AwayTeam { get; set; }
    public int AwayTeamId { get; set; }
    public ICollection<TicketPairTip> TicketPairTips { get; set; }
    public ICollection<PairTip> PairTips { get; set; }
  }
}
