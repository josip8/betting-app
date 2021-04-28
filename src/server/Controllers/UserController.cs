using data.Context;
using data.EfCoreModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.Models;
using server.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace server.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly BettingAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;
    public UserController(BettingAppDbContext context, UserManager<AppUser> userManager, IAuthService authService)
    {
      _context = context;
      _userManager = userManager;
      _authService = authService;
    }

    /// <summary>
    /// Generates JWT for user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] AuthUser model)
    {
      if (!ModelState.IsValid) return BadRequest("Invalid model");
      var user = await _userManager.FindByNameAsync(model.Username);
      if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
      {
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
          authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _authService.GenerateTokens(model.Username, authClaims, DateTime.Now);
        return Ok(token);
      }
      return Unauthorized();
    }

    /// <summary>
    /// Registers new user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Register(AuthUser model)
    {
      var newUser = new AppUser
      {
        UserName = model.Username,
        // to do - mail confirmation
        EmailConfirmed = true,
        WalletAmount = 0
      };
      var result = await _userManager.CreateAsync(newUser, model.Password);
      if (!result.Succeeded) return BadRequest(result.Errors);

      return Ok();
    }

    /// <summary>
    /// Adds specified amount to user wallet
    /// </summary>
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

    /// <summary>
    /// Withdraw specified amount from user wallet
    /// </summary>
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
