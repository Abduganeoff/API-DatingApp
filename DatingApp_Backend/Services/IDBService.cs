using DatingApp_Backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_Backend.Services
{
    public interface IDBService
    {

        Task<Users> Register(Users user, string password);
        Task<Users> Loging(string userName, string password);
        Task<bool> UserExist(string name);
    }
}
