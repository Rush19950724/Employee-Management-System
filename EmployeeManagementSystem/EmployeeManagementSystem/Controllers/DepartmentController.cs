using System.Linq;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly EmployeeContext _context;
        public DepartmentController(IConfiguration configuration, EmployeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var departmentList = _context.Departments.ToList();
            return new JsonResult(departmentList);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            var dbEntry = _context.Departments.Any(x => x.DepartmentName == department.DepartmentName);
            if (!dbEntry)
            {
                var result = _context.Departments.Add(department);
                _context.SaveChanges();
                return new JsonResult("Department added Successfully!");
            }
            return new JsonResult("Record Exists");
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            var dbEntry = _context.Departments.Find(department.DepartmentId); ;
            if (dbEntry != null)
            {

                dbEntry.DepartmentName = department.DepartmentName;
                _context.SaveChanges();
                return new JsonResult("Department Updated Successfully!");
            }
            return new JsonResult("There are no any changes!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int Id)
        {
            var dbEntry = _context.Departments.FirstOrDefault(x=>x.DepartmentId == Id); ;
            if (dbEntry != null)
            {
                _context.Remove(dbEntry);
                _context.SaveChanges();
                return new JsonResult("Department Deleted Successfully!");
            }
            return new JsonResult("Unsuccessful! Try Again");
        }

    }
}
