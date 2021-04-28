using data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace server.Controllers
{
  [Authorize]
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class PairController : ControllerBase
  {
    private readonly BettingAppDbContext _context;
    public PairController(BettingAppDbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Returns all active pairs you can bet on
    /// </summary>
    [HttpGet]
    public IActionResult AllActivePairs()
    {
      var pairs = _context.Pair.Where(x => x.GameStart > DateTime.Now)
        .Include(x => x.PairTips)
        .Include(x => x.HomeTeam)
        .Include(x => x.AwayTeam);

      return Ok(pairs.Select(x => new
        {
          HomeTeam = x.HomeTeam.Name,
          AwayTeam = x.AwayTeam.Name,
          x.Id,
          x.GameStart,
          PairTips = x.PairTips.Select(pt => new
          {
            pt.Id,
            pt.Coefficient,
            pt.Status,
            pt.Tip.TipName
          })
        }
      ));
    }
  }
}
