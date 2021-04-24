using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.EfCoreModels
{
  public class EntityBase
  {
    public int Id { get; set; }

    [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataType(DataType.Date)]
    public DateTime Created { get; set; }

    [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DataType(DataType.Date)]
    public DateTime Modified { get; set; }
  }
}
