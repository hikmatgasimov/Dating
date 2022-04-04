using DatingApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;
       
        public BuggyController(DataContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            //return "Unauthorized";
            return "secret text";
        }
        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _context.AppUser.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }  
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            try
            {
                var thing = _context.AppUser.Find(-1);

                var thingToReturn = thing.ToString();

                return thingToReturn;
            }
            catch (Exception)
            {
                return StatusCode(500, "Computer say no");
            }
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}
