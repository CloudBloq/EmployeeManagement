using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Model
{
    public class EmployeeManagementDBContext : DbContext
    {
        public EmployeeManagementDBContext(DbContextOptions<EmployeeManagementDBContext> options) : base(options)
        {
        }

        public DbSet<Employee> employee { get; set; }

    }
}
