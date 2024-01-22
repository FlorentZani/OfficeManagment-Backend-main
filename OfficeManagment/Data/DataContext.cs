using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeManagment.Model;

namespace OfficeManagment.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<UserProjects> UserProjects { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProjects>()
                .HasOne(b => b.User)
                .WithMany(a => a.UserProjects)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<UserProjects>()
                .HasOne(b => b.Projects)
                .WithMany(a => a.UserProjects)
                .HasForeignKey(a => a.ProjectId);

            //modelBuilder.Entity<UserProjects>()
            //    .HasOne(up => up.Position)
            //    .WithMany(p => p.UserProjects)
            //    .HasForeignKey(up => up.PositionIds);


            modelBuilder.Entity<UserProjects>()
                .Property(up => up.PositionIds)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<Guid>>(v))
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<UserRole>()
                .HasOne(b => b.User)
                .WithMany(a => a.Role)
                .HasForeignKey(a => a.UserId);
            
            
            modelBuilder.Entity<UserRole>()
                .HasOne(b => b.Roles)
                .WithMany(a => a.Users)
                .HasForeignKey(a => a.RoleId);

            


        }




    }



}
