using data.Context;
using data.EfCoreModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly BettingAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    public UserController(BettingAppDbContext context, UserManager<AppUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [HttpPut("{amount:decimal}")]
    public async Task<IActionResult> AddToWallet(decimal amount)
    {
      var user = await _userManager.FindByEmailAsync("test@test.com");
      if(amount <= 0)
      {
        return BadRequest("Cannot add zero or negative value");
      }

      var transaction = new Transaction
      {
        User = user,
        Amount = amount,
        NewAmount = user.WalletAmount + amount,
        OldAmount = user.WalletAmount,
        TransactionType = (int)TransactionType.Payment
      };

      await _context.Transaction.AddAsync(transaction);
      user.WalletAmount += amount;
      _context.Users.Update(user);

      _context.SaveChanges();
      return Ok(user.WalletAmount);
    }

    [HttpPut("{amount:decimal}")]
    public async Task<IActionResult> WithdrawFromWallet(decimal amount)
    {
      var user = await _userManager.FindByEmailAsync("test@test.com");
      if (amount <= 0)
      {
        return BadRequest("Cannot add zero or negative value");
      }
      if(amount > user.WalletAmount)
      {
        return BadRequest("Insufficient funds");
      }

      var transaction = new Transaction
      {
        User = user,
        Amount = -amount,
        NewAmount = user.WalletAmount - amount,
        OldAmount = user.WalletAmount,
        TransactionType = (int)TransactionType.Withdraw
      };
      await _context.Transaction.AddAsync(transaction);

      user.WalletAmount -= amount;
      _context.Users.Update(user);

      _context.SaveChanges();
      return Ok(user.WalletAmount);
    }
  }
}
