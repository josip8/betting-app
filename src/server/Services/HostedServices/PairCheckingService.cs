using data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using data.EfCoreModels;
using static server.Services.Helpers.RandomService;

namespace server.Services.HostedServices
{
  public class PairCheckingService : BackgroundService
  {
    private readonly BettingAppDbContext _context;
    public PairCheckingService(IServiceScopeFactory factory)
    {
      _context = factory.CreateScope().ServiceProvider.GetRequiredService<BettingAppDbContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var currentDate = DateTime.Now;
        var getPairs = _context.PairTip.Where(x => x.Pair.GameStart < currentDate  && x.Status == (int)PairStatus.Pending).Include(x => x.Tip)
          .Include(x => x.Pair).ThenInclude(x => x.HomeTeam).ThenInclude(x => x.Sport).AsEnumerable().GroupBy(x => x.PairId);

        foreach(var item in getPairs)
        {
          GenerateResult(item.ToList());
        }

        await Task.Delay(1000 * 60, stoppingToken);
      }
    }

    private void GenerateResult(List<PairTip> pairTips)
    {
      if (pairTips.FirstOrDefault().Pair.HomeTeam.Sport.Name == "Football")
      {
        int outcome = GenerateRandomInt(1, 4);
        switch (outcome)
        {
          case 1:
            foreach (var item in pairTips)
            {
              item.Status = item.Tip.TipName.Contains("1") ? (int)PairStatus.Success : (int)PairStatus.Fail;
              _context.PairTip.Update(item);
            }
            break;
          case 2:
            foreach (var item in pairTips)
            {
              item.Status = item.Tip.TipName.Contains("2") ? (int)PairStatus.Success : (int)PairStatus.Fail;
              _context.PairTip.Update(item);
            }
            break;
          case 3:
            foreach (var item in pairTips)
            {
              item.Status = item.Tip.TipName.Contains("X") ? (int)PairStatus.Success : (int)PairStatus.Fail;
              _context.PairTip.Update(item);
            }
            break;
        }
      }
      else if(pairTips.FirstOrDefault().Pair.HomeTeam.Sport.Name == "Tennis")
      {
        int outcome = GenerateRandomInt(1, 4);
        switch (outcome)
        {
          case 1:
            foreach (var item in pairTips)
            {
              item.Status = item.Tip.TipName.Contains("1") ? (int)PairStatus.Success : (int)PairStatus.Fail;
              _context.PairTip.Update(item);
            }
            break;
          case 2:
            foreach (var item in pairTips)
            {
              item.Status = item.Tip.TipName.Contains("2") ? (int)PairStatus.Success : (int)PairStatus.Fail;
              _context.PairTip.Update(item);
            }
            break;
        }
      }

      _context.SaveChanges();
    }
  }
}
