using data.Context;
using data.EfCoreModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        _  = await _context.Sport.AddAsync(football);
        var tennis = new Sport { Name = "Tennis" };
        _ = await _context.Sport.AddAsync(tennis);


        // insert sport teams
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Barcelona", Description = "Football club from Spain", Sport = football });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Real Madrid", Description = "Football club from Spain", Sport = football });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Atletico Madrid", Description = "Football club from Spain", Sport = football });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Sevilla", Description = "Football club from Spain", Sport = football });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Valencia", Description = "Football club from Spain", Sport = football });

        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Novak Djokovic", Description = "No. 1 on ATP rankings", Sport = tennis });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Daniil Medvedev", Description = "No. 1 on ATP rankings", Sport = tennis });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Rafael Nadal", Description = "No. 1 on ATP rankings", Sport = tennis });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Roger Federer", Description = "No. 1 on ATP rankings", Sport = tennis });
        _ = await _context.SportTeam.AddAsync(new SportTeam { Name = "Dominic Thiem", Description = "No. 1 on ATP rankings", Sport = tennis });


        // insert tips
        var tip1 = new Tip { TipName = "1", Description = "Host victory" };
        var tip2 = new Tip { TipName = "2", Description = "Guest victory" };
        var tipX = new Tip { TipName = "X", Description = "Draw" };
        var tip1X = new Tip { TipName = "1X", Description = "Host victory or draw" };
        var tip2X = new Tip { TipName = "2X", Description = "Guest victory or draw" };
        var tip12 = new Tip { TipName = "12", Description = "Not a draw" };
        _ = await _context.Tip.AddAsync(tip1);
        _ = await _context.Tip.AddAsync(tip2);
        _ = await _context.Tip.AddAsync(tipX);
        _ = await _context.Tip.AddAsync(tip1X);
        _ = await _context.Tip.AddAsync(tip2X);
        _ = await _context.Tip.AddAsync(tip12);

        // insert sporttips
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip1 });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip2 });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tipX });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip1X });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip2X });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = football, Tip = tip12 });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = tennis, Tip = tip1 });
        _ = await _context.SportTip.AddAsync(new SportTip { Sport = tennis, Tip = tip2 });

        _context.SaveChanges();
      }

      return Ok(tickets);
    }


  }
}
