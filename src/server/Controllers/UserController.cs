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
  [Route("api/[controller]")]
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

    [HttpPost]
    public async Task<IActionResult> AddToWallet([FromBody] decimal amount)
    {
      var user = await _userManager.FindByEmailAsync("test@test.com");
      if(amount < 0)
      {
        return BadRequest("Cannot add negative value");
      }
      user.WalletAmount += amount;
      _context.Users.Update(user);
      _context.SaveChanges();
      return Ok(user.WalletAmount);
    }
  }
}
