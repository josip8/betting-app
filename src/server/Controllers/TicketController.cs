using data.EfCoreModels;
using Microsoft.AspNetCore.Http;
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

    [HttpGet]
    public async Task<IActionResult> NewTicket([FromBody] NewTicket newTicket)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var ticket = new Ticket
      {
        Status = (int) TicketStatus.Pending,
        PrizeAmount = 100
      };
      return Ok();
    }
  }
}
