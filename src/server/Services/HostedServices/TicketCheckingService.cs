using data.Context;
using data.EfCoreModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace server.Services.HostedServices
{
  public class TicketCheckingService : BackgroundService
  {
    private readonly BettingAppDbContext _context;
    public TicketCheckingService(IServiceScopeFactory factory)
    {
      _context = factory.CreateScope().ServiceProvider.GetRequiredService<BettingAppDbContext>();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var ticketsToCheck = _context.Ticket.Where(x => x.Status == (int)TicketStatus.Pending
          && x.TicketPairTips.All(y => y.PairTip.Status != (int)PairStatus.Pending))
          .Include(x => x.TicketPairTips).ThenInclude(x => x.PairTip).Include(x => x.User);

        foreach(var item in ticketsToCheck)
        {
          if(item.TicketPairTips.Any(x => x.PairTip.Status == (int)PairStatus.Fail))
          {
            item.Status = (int)TicketStatus.Fail;
          }
          else
          {
            item.Status = (int)TicketStatus.Success;

            var transaction = new Transaction
            {
              User = item.User,
              Amount = item.PrizeAmount,
              NewAmount = item.User.WalletAmount + item.PrizeAmount,
              OldAmount = item.User.WalletAmount,
              TransactionType = (int)TransactionType.TicketWin
            };
            item.User.WalletAmount += item.PrizeAmount;
            await _context.Transaction.AddAsync(transaction);
            _context.Users.Update(item.User);
          }
        }

        _context.SaveChanges();
        await Task.Delay(1000 * 60, stoppingToken);
      }
    }
  }
}
