using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp_Backend.DTOs.Response;
using DatingApp_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp_Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IDatingRepository _dating;
        private readonly IMapper _mapper;

        public UserController(IDatingRepository dating, IMapper mapper)
        {
            _dating = dating;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _dating.GetUser(id);
            var userToReturn = _mapper.Map<UserDetailedResponse>(user);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dating.GetUsers();
            var userToReturn = _mapper.Map<IEnumerable<UsersToListResponse>>(users);

            return Ok(userToReturn);
        }
    }
}