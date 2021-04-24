using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class Tip : EntityBase
  {
    public string TipName { get; set; }
    public string Description { get; set; }
  }
}
