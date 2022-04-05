using BAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical_22.DataContext;
using Practical_22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practical_22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly DepartmentFactory departmentFactory;
       
        public EmployeeController(AppDbContext context, DepartmentFactory departmentFactory)
        {
            this.context = context;
            this.departmentFactory = departmentFactory;
        }
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee emp)
        {
            if (emp.Id == 0)
            {
                await context.Employees.AddAsync(emp);
                await context.SaveChangesAsync();
                return Ok(emp);
            }
            return BadRequest("Error in Adding Employee");
        }
        [HttpGet()]
        public async Task<ActionResult> GetEmployees(int? id)
        {
            if (id == null)
            {
                var result = await context.Employees.Include(x => x.Department).ToListAsync();
                await context.SaveChangesAsync();
                return Ok(result);
            }
            else if (id != null)
            {
                var result= await context.Employees.Include(x => x.Department).Where(x => x.Id == id).ToListAsync();
                return Ok(result);
            }
            return NotFound("Data Not found");
        }
        [HttpPut("EditEmployee/{id}")]
        public async Task<IActionResult> EditEmployee(int id, [FromBody] Employee emp)
        {
            if (id == emp.Id)
            {
                context.Employees.Update(emp);
                await context.SaveChangesAsync();
                return Ok(emp);
            }
            return BadRequest("Error in Updation of Employee");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.Status = true;
                context.Employees.Update(employee);
                await context.SaveChangesAsync();
                return Ok("Employee Delete Successfully");
            }
            return NotFound("Employee Can not Deleted");
        }
        [HttpGet("OvertimePay")]
        public async Task<ActionResult> OvertimePay(int id,int hour)
        {
            if (id != 0)
            {
                var deptname = context.Employees.Include(x => x.Department).Where(x=> x.Id==id).Select(x => x.Department.DepartmentName).FirstOrDefault();
               // var deptname =  await context.Employees.Where(x=> x.Id==id).Join(context.Departments, emp => emp.DepartmentId, dept => dept.Id, (emp, dept) => new { dept.DepartmentName}).FirstOrDefaultAsync();
               // var DepartMentName = deptname.ToString();
                var result = departmentFactory.Getobj(deptname);
                var overtimepay=  result.MyOverTimePay(hour);
                return Ok(overtimepay);
            }
            return NotFound("Data Not found");
        }
    }
}
