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
using Microsoft.EntityFrameworkCore;

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
      float coefficient = 1;

      var ticket = new Ticket
      {
        Status = (int)TicketStatus.Pending,
        MoneyPaid = newTicket.MoneyAmount
      };

      foreach (var pairTipId in newTicket.PairTips)
      {
        // validate and calculate
        var pairTip = _context.PairTip.Include(x => x.Pair).FirstOrDefault(x => x.Id == pairTipId);
        if (pairTip?.Status != (int)PairStatus.Pending || pairTip.Coefficient < 1) return BadRequest("Pair/tip not valid");
        if (newTicket.SuperTip == pairTip.Id) 
        {
          coefficient *= pairTip.Pair.SpecialOfferModifier;
        } 
        coefficient *= pairTip.Coefficient;
        await _context.TicketPairTip.AddAsync(new TicketPairTip { Ticket = ticket, PairTip = pairTip });
      }

      ticket.PrizeAmount = (newTicket.MoneyAmount - newTicket.MoneyAmount * (decimal)0.05) * (decimal)coefficient;
      ticket.ManipulativeCosts = newTicket.MoneyAmount * (decimal)0.05;
      ticket.Coefficient = coefficient;
      await _context.Ticket.AddAsync(ticket);

      _context.SaveChanges();
      return Ok();
    }

  }
}
