using Microsoft.EntityFrameworkCore;
namespace data.Context
{
  public class BettingAppDbContext : DbContext
  {
    public BettingAppDbContext()
    {
    }

    public BettingAppDbContext(DbContextOptions<BettingAppDbContext> options) : base(options) { }
  }
}