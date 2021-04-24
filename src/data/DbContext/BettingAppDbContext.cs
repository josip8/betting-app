using data.EfCoreModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace data.Context
{
  public class BettingAppDbContext : IdentityDbContext<AppUser>
  {
    public BettingAppDbContext()
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseNpgsql("Server=localhost;Port=5432;Database=betting-app;User ID=test;Password=test;");
    }

    public BettingAppDbContext(DbContextOptions<BettingAppDbContext> options) : base (options) {}

  }
}