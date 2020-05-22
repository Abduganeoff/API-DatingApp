using DatingApp_Backend.Data;
using DatingApp_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_Backend.Services
{
    public class DatingRepository : IDatingRepository
    {
        private readonly UserDbContext _userDb;

        public DatingRepository(UserDbContext userDb)
        {
            _userDb = userDb;
        }
        public void Add<T>(T entity) where T : class
        {
            _userDb.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _userDb.Remove(entity);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _userDb.Photos.FirstOrDefaultAsync(opt => opt.Id == id);

            return photo;
        }

        public async Task<Users> GetUser(int id)
        {
            var userToReturn = await _userDb.Users.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == id);

            return userToReturn;
        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            var usersToReturn = await  _userDb.Users.Include(p => p.Photos).ToListAsync();

            return usersToReturn;
        }

        public async Task<bool> SaveAll()
        {
            return await _userDb.SaveChangesAsync() > 0;
        }
    }
}
