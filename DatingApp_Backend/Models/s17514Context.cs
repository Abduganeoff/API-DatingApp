using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DatingApp_Backend.Models
{
    public partial class s17514Context : DbContext
    {
        public s17514Context()
        {
        }

        public s17514Context(DbContextOptions<s17514Context> options)
            : base(options)
        {
        }


        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Value> Value { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=s17514;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                .HasName("Users_pk");

                entity.Property(e => e.PasswordHash).HasMaxLength(1024);

                entity.Property(e => e.PasswordSalt).HasMaxLength(1024);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Value>(entity =>
            {
                entity.HasKey(e => e.Index)
                .HasName("Value_pk");

                entity.Property(e => e.Val)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
