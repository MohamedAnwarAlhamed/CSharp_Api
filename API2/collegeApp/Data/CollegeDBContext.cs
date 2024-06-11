using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) :
            base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder
            //     .Entity<Student>()
            //     .HasData(new List<Student>()
            //     {
            //         new Student {
            //             Id = 1,
            //             StudentName = "Venkat",
            //             Address = "India",
            //             Email = "venkat@gmail.com",
            //             DOB = new DateTime(2022, 12, 12)
            //         },
            //         new Student {
            //             Id = 2,
            //             StudentName = "Nehanth",
            //             Address = "India",
            //             Email = "nehanth@gmail.com",
            //             DOB = new DateTime(2022, 06, 12)
            //         }
            //     });
            // modelBuilder
            //     .Entity<Student>(entity =>
            //     {
            //         entity.Property(n => n.StudentName).IsRequired();
            //         entity.Property(n => n.StudentName).HasMaxLength(250);
            //         entity
            //             .Property(n => n.Address)
            //             .IsRequired(false)
            //             .HasMaxLength(500);
            //         entity
            //             .Property(n => n.Email)
            //             .IsRequired()
            //             .HasMaxLength(250);
            //     });
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
        }
    }
}
