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
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers
{
  [Authorize]
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

    /// <summary>
    /// Creates new ticket
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> New([FromBody] NewTicket newTicket)
    {
      if (!ModelState.IsValid) return BadRequest("Invalid model");
      var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
      var user = _context.Users.FirstOrDefault(user => user.UserName == username.Value);
      if (newTicket.MoneyAmount > user.WalletAmount) return BadRequest("Insufficient funds");
      if (newTicket.MoneyAmount <= 0) return BadRequest("Money amount must be greater than zero");
      var allPairs = newTicket.PairTips.Select(x => _context.Pair.FirstOrDefault(p => p.PairTips.Any(pt => p == pt.Pair)).Id);
      if (allPairs.Distinct().Count() != allPairs.Count()) return BadRequest("Duplicate pairs");

      float coefficient = 1;
      var ticket = new Ticket
      {
        Status = (int)TicketStatus.Pending,
        MoneyPaid = newTicket.MoneyAmount,
        User = user
      };

      foreach (var pairTipId in newTicket.PairTips)
      {
        // validate and calculate
        var pairTip = _context.PairTip.Include(x => x.Pair).FirstOrDefault(x => x.Id == pairTipId);
        if (pairTip?.Status != (int)PairStatus.Pending || pairTip.Coefficient < 1) return BadRequest("Pair/tip not valid");
        if (newTicket.SuperTip == pairTip.Id)
        {
          coefficient *= pairTip.Pair.SpecialOfferModifier;
          ticket.SpecialOfferPair = pairTip.Pair;
        }
        coefficient *= pairTip.Coefficient;
        await _context.TicketPairTip.AddAsync(new TicketPairTip { Ticket = ticket, PairTip = pairTip });
      }

      ticket.PrizeAmount = (newTicket.MoneyAmount - newTicket.MoneyAmount * (decimal)0.05) * (decimal)coefficient;
      ticket.ManipulativeCosts = newTicket.MoneyAmount * (decimal)0.05;
      ticket.Coefficient = coefficient;
      await _context.Ticket.AddAsync(ticket);

      var transaction = new Transaction
      {
        User = user,
        Amount = -newTicket.MoneyAmount,
        NewAmount = user.WalletAmount - newTicket.MoneyAmount,
        OldAmount = user.WalletAmount,
        TransactionType = (int)TransactionType.TicketPayment
      };
      await _context.Transaction.AddAsync(transaction);
      user.WalletAmount = transaction.NewAmount;
      _context.Users.Update(user);

      _context.SaveChanges();
      return Ok();
    }


    /// <summary>
    /// Returns ticket info by ticket id
    /// </summary>
    [HttpGet("{id:int}")]
    public IActionResult Info(int id)
    {
      var ticket = _context.Ticket
        .Include(x => x.TicketPairTips).ThenInclude(x => x.PairTip).ThenInclude(x => x.Tip)
        .Include(x => x.TicketPairTips).ThenInclude(x => x.PairTip).ThenInclude(x => x.Pair).ThenInclude(x => x.AwayTeam)
        .Include(x => x.TicketPairTips).ThenInclude(x => x.PairTip).ThenInclude(x => x.Pair).ThenInclude(x => x.HomeTeam)
        .Include(x => x.TicketPairTips).ThenInclude(x => x.PairTip).ThenInclude(x => x.Pair).ThenInclude(x => x.PairTips).ThenInclude(x => x.Tip)
        .FirstOrDefault(x => x.Id == id);

      if (ticket == null) return BadRequest("Ticket with provided id does not exist");
      return Ok(new
      {
        ticket.Id,
        ticket.Status,
        StatusString = ((TicketStatus)ticket.Status).ToString(),
        ticket.PrizeAmount,
        ticket.MoneyPaid,
        ticket.ManipulativeCosts,
        ticket.Coefficient,
        TicketPairTips = ticket.TicketPairTips.Select(x => new
        {
          x.PairTipId,
          x.PairTip.Tip.TipName,
          x.PairTip.Pair.GameStart,
          HomeTeam = x.PairTip.Pair.HomeTeam.Name,
          AwayTeam = x.PairTip.Pair.AwayTeam.Name,
          AllTips = x.PairTip.Pair.PairTips.Select(x => new
          {
            x.Tip.TipName,
            x.Coefficient,
            x.Id,
            x.Status,
            StatusString = ((PairStatus)ticket.Status).ToString()
          })
        })
      });
    }

    /// <summary>
    /// Returns list off all user tickets
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> MyTickets()
    {
      var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
      var user = _context.Users.FirstOrDefault(user => user.UserName == username.Value);
      var myTickets = _context.Ticket.Where(x => x.User == user).Select(x => new
      {
        x.MoneyPaid,
        x.Id,
        x.Created,
        x.PrizeAmount,
        x.ManipulativeCosts,
        x.Status,
        StatusString = ((TicketStatus)x.Status).ToString()
      });
      return Ok(myTickets);
    }
  }
}
