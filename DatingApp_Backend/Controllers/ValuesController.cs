using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class ValuesController : ControllerBase
    {
        private readonly s17514Context _dbContext;

        public ValuesController(s17514Context dbContext)
        {
            _dbContext = dbContext;
        }

        
        [HttpGet]
        public IActionResult GetValues()
        {
            var values =  _dbContext.Value.ToList();

            return Ok(values);

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetValue(int id)
        {
            var value = await _dbContext.Value.FirstOrDefaultAsync(c => c.Index == id);

            return Ok(value);
        }
    }
}