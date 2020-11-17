using Employee.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.DataAccess.Repository
{
    public class EmployeeDAO : IEmployeeDAO
    {
        private readonly EmployeeManagementDBContext dbContext;

        public EmployeeDAO(EmployeeManagementDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Model.Employee> CreateEmployee(Model.Employee employee)
        {
            await dbContext.employee.AddAsync(employee);
            dbContext.SaveChanges();
            return employee;
        }

        public async Task<Model.Employee> GetEmployeeById(Guid? id)
        {
            Model.Employee employee = await dbContext.employee.FindAsync(id);

            if (employee == null)
            {
                return null;
            }

            return employee;
        }

        public List<Model.Employee> GetAll()
        {
            return dbContext.employee.ToList();
        }

        public async Task<Model.Employee> UpdateEmployee(Model.Employee employee)
        {
            var checkEmployee = await GetEmployeeById(employee.id);
            if (checkEmployee == null)
                return null;

            var local = dbContext.Set<Model.Employee>()
                .Local
                .FirstOrDefault(entry => entry.id.Equals(employee.id));

            if (local != null)
            {
                // detach
                dbContext.Entry(local).State = EntityState.Detached;
            }

            dbContext.Entry(employee).State = EntityState.Modified;

            dbContext.employee.Update(employee);
            await dbContext.SaveChangesAsync();

            return employee;
        }
    }
}
