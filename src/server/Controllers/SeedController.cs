using data.Context;
using data.EfCoreModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace server.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class SeedController : ControllerBase
  {
    private readonly BettingAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    public SeedController(BettingAppDbContext context, UserManager<AppUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> SeedUserSportAndTips()
    {
      var tickets = _context.Ticket.ToList();

      var user = await _userManager.FindByEmailAsync("test@test.com");
      if (user == null)
      {
        var newUser = new AppUser
        {
          UserName = "test",
          Email = "test@test.com",
          EmailConfirmed = true,
          WalletAmount = 0
        };
        var result = await _userManager.CreateAsync(newUser, "Test123!");
        if (!result.Succeeded) return BadRequest(result.Errors);
      }

      if (!_context.Sport.Any() && !_context.Tip.Any())
      {
        var football = new Sport { Name = "Football" };
        await _context.Sport.AddAsync(football);
        var tennis = new Sport { Name = "Tennis" };
        await _context.Sport.AddAsync(tennis);


        // insert sport teams
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Barcelona", Description = "Football club from Spain", Sport = football });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Real Madrid", Description = "Football club from Spain", Sport = football });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Atletico Madrid", Description = "Football club from Spain", Sport = football });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Sevilla", Description = "Football club from Spain", Sport = football });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Valencia", Description = "Football club from Spain", Sport = football });

        await _context.SportTeam.AddAsync(new SportTeam { Name = "Novak Djokovic", Description = "No. 1 on ATP rankings", Sport = tennis });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Daniil Medvedev", Description = "No. 1 on ATP rankings", Sport = tennis });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Rafael Nadal", Description = "No. 1 on ATP rankings", Sport = tennis });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Roger Federer", Description = "No. 1 on ATP rankings", Sport = tennis });
        await _context.SportTeam.AddAsync(new SportTeam { Name = "Dominic Thiem", Description = "No. 1 on ATP rankings", Sport = tennis });


        // insert tips
        var tip1 = new Tip { TipName = "1", Description = "Host victory" };
        var tip2 = new Tip { TipName = "2", Description = "Guest victory" };
        var tipX = new Tip { TipName = "X", Description = "Draw" };
        var tip1X = new Tip { TipName = "1X", Description = "Host victory or draw" };
        var tip2X = new Tip { TipName = "2X", Description = "Guest victory or draw" };
        var tip12 = new Tip { TipName = "12", Description = "Not a draw" };
        await _context.Tip.AddAsync(tip1);
        await _context.Tip.AddAsync(tip2);
        await _context.Tip.AddAsync(tipX);
        await _context.Tip.AddAsync(tip1X);
        await _context.Tip.AddAsync(tip2X);
        await _context.Tip.AddAsync(tip12);

        // insert sporttips
        await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip1 });
        await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip2 });
        await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tipX });
        await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip1X });
        await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip2X });
        await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip12 });
        await _context.SportTip.AddAsync(new SportTip { Sport = tennis, Tip = tip1 });
        await _context.SportTip.AddAsync(new SportTip { Sport = tennis, Tip = tip2 });

        _context.SaveChanges();
      }

      return Ok(tickets);
    }

    [HttpGet]
    public async Task<IActionResult> GeneratePairs()
    {
      // generate 5 pairs for football and 5 pairs for tennis - in next 30 mins
      foreach (int value in Enumerable.Range(1, 5))
      {
        await GeneratePairsForSport("Football");
        await GeneratePairsForSport("Tennis");
      }
      return Ok();
    }

    private async Task GeneratePairsForSport(string sportName)
    {
      var sportTeams = _context.SportTeam.Where(x => x.Sport.Name == sportName);
      var teamHome = sportTeams.Skip(GenerateRandomInt(0, sportTeams.Count())).Take(1).FirstOrDefault();
      var teamAway = sportTeams.Where(x => x != teamHome).Skip(GenerateRandomInt(0, sportTeams.Count() - 1)).Take(1).FirstOrDefault();

      bool IsSpecialOffer = GenerateRandomInt(0, 2) == 0;

      var newPair = new Pair
      {
        HomeTeam = teamHome,
        AwayTeam = teamAway,
        SpecialOffer = IsSpecialOffer,
        SpecialOfferModifier = 1.1f,
        GameStart = DateTime.Now.AddMinutes(GenerateRandomInt(5, 30))
      };
      await _context.Pair.AddAsync(newPair);

      float tip1 = GenerateRandomFloat(GenerateRandomInt(1, 5));

      foreach (var sportTip in _context.SportTip.Where(x => x.SportId == teamHome.SportId).Include(x => x.Tip))
      {
        var newPairTip = new PairTip
        {
          Pair = newPair,
          Tip = sportTip.Tip,
          Coefficient = CalculateCoefficient(sportTip.Tip.TipName, tip1)
        };

        await _context.PairTip.AddAsync(newPairTip);
      }
      _context.SaveChanges();
    }

    private static float CalculateCoefficient(string tipName, float tip1)
    {
      float coefficient;
      switch (tipName)
      {
        case "1":
          coefficient = tip1;
          break;
        case "1X":
          coefficient = (float)(tip1 / 1.45);
          break;
        case "2":
          coefficient = (float)(7 / tip1);
          break;
        case "2X":
          coefficient = (float)(3 / tip1);
          break;
        case "X":
          coefficient = GenerateRandomFloat(GenerateRandomInt(2, 4));
          break;
        case "12":
          coefficient = 1.25f;
          break;
        default:
          throw new Exception("Tip does not exist");
      }

      return coefficient > 1 ? coefficient : 0;
    }

    private static int GenerateRandomInt(int a, int b)
    {
      Random r = new();
      int randomInteger = r.Next(a, b);
      return randomInteger;
    }

    private static float GenerateRandomFloat(int addToFloat)
    {
      Random r = new();
      double randomDouble = r.NextDouble() + addToFloat;
      return (float)randomDouble;
    }

  }
}
