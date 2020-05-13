using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp_Backend.Data;
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
        private readonly UserDbContext _dbContext;

        public ValuesController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IActionResult GetValues()
        {
            var values =  _dbContext.Values.ToList();

            return Ok(values);

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetValue(int id)
        {
            var value = await _dbContext.Values.FirstOrDefaultAsync(c => c.Id == id);

            return Ok(value);
        }
    }
}