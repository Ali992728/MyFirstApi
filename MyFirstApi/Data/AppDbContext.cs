using Microsoft.EntityFrameworkCore;
using MyFirstApi.Entities;

namespace MyFirstApi.Data
{
    public class AppDbContext:DbContext                                            //AppDbContext bridge er moto kaj kore between c# code and database.
    {                                           
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)  //database er type ar connection string ney options er madhome 
        {                                                                          //base(options) er maddhome DbContext e pathano hoy
        }

        public DbSet<User> AccountUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
