using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp_Backend.DTOs;
using DatingApp_Backend.DTOs.Request;
using DatingApp_Backend.DTOs.Response;
using DatingApp_Backend.Helpers;
using DatingApp_Backend.Models;
using DatingApp_Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp_Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IDBService _dbContext;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthenticationController(IDBService dbContext, IConfiguration config, IMapper mapper)
        {
            _dbContext = dbContext;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {

            request.UserName = request.UserName.ToLower();

            if (await _dbContext.UserExist(request.UserName))
                return BadRequest($"{request.UserName} already exists");

            var userToCReate = new Users
            {
                UserName = request.UserName
            };

            var createdUser = await _dbContext.Register(userToCReate, request.Password);

            return StatusCode(201);

        }


        [HttpPost("log")]
        public async Task<IActionResult> Loging(LogingRequest request)
        {

            var userFromLog = await _dbContext.Loging(request.UserName.ToLower(), request.Password);

            if (userFromLog == null) return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromLog.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromLog.UserName),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Rizo",
                Audience = "Users",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user = _mapper.Map<UserDetailedResponse>(userFromLog);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });



        }
    }
}