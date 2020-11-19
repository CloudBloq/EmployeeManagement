using Employee.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeDAO _employeeDAO;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeDAO employeeDAO)
        {
            _logger = logger;
            _employeeDAO = employeeDAO;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(DataAccess.Model.Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee not passed");

            var createdEmployee = await _employeeDAO.CreateEmployee(employee);

            if (createdEmployee != null)
            {
                return new CreatedAtActionResult("GetEmployee", "Employee", new { createdEmployee.id }, createdEmployee);
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid? id)
        {
            if (id == null || id.Value == Guid.Empty)
                return BadRequest("Id must not be empty");

            var employee = await _employeeDAO.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound($"Employee with Id {id} doesn't exit");
            }

            return Ok(employee);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allEmployee = _employeeDAO.GetAll();

            if (allEmployee.Count != 0)
                return Ok(allEmployee);

            return NotFound("No employee exit yet.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(DataAccess.Model.Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee not passed");

            var updatedEmployee = await _employeeDAO.UpdateEmployee(employee);

            if (updatedEmployee == null)
                return BadRequest("Error occured please check the input data and try again.");

            return Ok(updatedEmployee);
        }
    }
}
