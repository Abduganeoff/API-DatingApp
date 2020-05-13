using DatingApp_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_Backend.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<Users> Users{ get; set; }
        public DbSet<Photo> Photos{ get; set; }
        public DbSet<Value> Values { get; set; }
        public UserDbContext(){}

        public UserDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
