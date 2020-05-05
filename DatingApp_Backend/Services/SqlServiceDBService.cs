using DatingApp_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp_Backend.Services
{
    public class SqlServiceDBService : IDBService
    {
        private readonly s17514Context _dbContex;

        public SqlServiceDBService(s17514Context dbContex)
        {
            _dbContex = dbContex;
        }

        public async Task<Users> Loging(string userName, string password)
        {
            var takenUser = await _dbContex.Users.FirstOrDefaultAsync(c => c.UserName ==userName);

            if (takenUser == null)
                return null;

            if (!VerifyPassword(password, takenUser.PasswordHash, takenUser.PasswordSalt))
                return null;

            return takenUser;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var computedHashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < computedHashPassword.Length; i++)
                {
                    if (computedHashPassword[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<Users> Register(Users user, string password)
        {
            byte[] _passwordHash, _saltHash;

            CreateHash(password, out _passwordHash, out _saltHash);

            user.PasswordHash = _passwordHash;
            user.PasswordSalt = _saltHash;

            await _dbContex.Users.AddAsync(user);
            await _dbContex.SaveChangesAsync();

            return user;
        }

        private void CreateHash(string password, out byte[] passwordHash, out byte[] saltHash)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                saltHash = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string name)
        {
            if (await _dbContex.Users.AnyAsync(c => c.UserName == name))
                return true;

                return false;

        }
    }
}
