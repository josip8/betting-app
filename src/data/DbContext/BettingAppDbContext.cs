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

    public BettingAppDbContext(DbContextOptions<BettingAppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Pair> Pair { get; set; }
    public DbSet<PairTip> PairTip { get; set; }
    public DbSet<Sport> Sport { get; set; }
    public DbSet<SportTeam> SportTeam { get; set; }
    public DbSet<SportTip> SportTip { get; set; }
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<TicketPairTip> TicketPairTip { get; set; }
    public DbSet<Tip> Tip { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Transaction>(entity =>
      {
        entity.HasOne(e => e.User).WithMany(u => u.Transactions).HasForeignKey(e => e.UserId).IsRequired(true);
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });

      modelBuilder.Entity<Pair>(entity =>
      {
        entity.HasOne(e => e.HomeTeam).WithMany(u => u.HomePairs).HasForeignKey(e => e.HomeTeamId).IsRequired(true);
        entity.HasOne(e => e.AwayTeam).WithMany(u => u.AwayPairs).HasForeignKey(e => e.AwayTeamId).IsRequired(true);
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });

      modelBuilder.Entity<PairTip>(entity =>
      {
        entity.HasOne(e => e.Pair).WithMany(u => u.PairTips).HasForeignKey(e => e.PairId).IsRequired(true);
        entity.HasOne(e => e.Tip).WithMany(u => u.PairTips).HasForeignKey(e => e.TipId).IsRequired(true);
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });

      modelBuilder.Entity<Sport>(entity =>
      {
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });

      modelBuilder.Entity<SportTeam>(entity =>
      {
        entity.HasOne(e => e.Sport).WithMany(u => u.SportTeams).HasForeignKey(e => e.SportId).IsRequired(true);
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });


      modelBuilder.Entity<SportTip>(entity =>
      {
        entity.HasKey(e => new { e.SportId, e.TipId });
        entity.HasOne(e => e.Sport).WithMany(u => u.SportTips).HasForeignKey(e => e.SportId).IsRequired(true);
        entity.HasOne(e => e.Tip).WithMany(u => u.SportTips).HasForeignKey(e => e.TipId).IsRequired(true);
      });

      modelBuilder.Entity<Ticket>(entity =>
      {
        entity.HasOne(e => e.SpecialOfferPair).WithMany(u => u.TicketsWithSpecialOffer).HasForeignKey(e => e.SpecialOfferPairId);
        entity.HasOne(e => e.User).WithMany(u => u.Tickets).HasForeignKey(e => e.UserId).IsRequired(true);
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });

      modelBuilder.Entity<TicketPairTip>(entity =>
      {
        entity.HasKey(e => new { e.TicketId, e.PairTipId });
        entity.HasOne(e => e.Ticket).WithMany(u => u.TicketPairTips).HasForeignKey(e => e.TicketId).IsRequired(true);
        entity.HasOne(e => e.PairTip).WithMany(u => u.TicketPairTips).HasForeignKey(e => e.PairTipId).IsRequired(true);
      });

      modelBuilder.Entity<Tip>(entity =>
      {
        entity.Property(s => s.Created).HasDefaultValueSql("now()");
        entity.Property(s => s.Modified).HasDefaultValueSql("now()");
      });
    }
  }
}