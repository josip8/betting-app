using data.Context;
using data.EfCoreModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class TicketController : ControllerBase
  {
    private readonly BettingAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    public TicketController(BettingAppDbContext context, UserManager<AppUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> NewTicket([FromBody] NewTicket newTicket)
    {
      if (!ModelState.IsValid) return BadRequest("Invalid model");
      var user = await _userManager.FindByEmailAsync("test@test.com");
      if (newTicket.MoneyAmount > user.WalletAmount) return BadRequest("Insufficient funds");


      foreach (var pairTipId in newTicket.PairTips)
      {
        var pairTip = _context.PairTip.FirstOrDefault(x => x.Id == pairTipId);
        if (pairTip?.Status != (int)PairStatus.Pending || pairTip.Coefficient < 1) return BadRequest("Pair/tip not valid");
      }

      var ticket = new Ticket
      {
        Status = (int) TicketStatus.Pending,
        MoneyPaid = newTicket.MoneyAmount,
        PrizeAmount = 100
      };
      return Ok();
    }

  }
}
