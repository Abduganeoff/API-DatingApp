using DatingApp_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_Backend.Services
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();
        Task<IEnumerable<Users>>GetUsers();
        Task<Users> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMain(int userId);


    }
}
