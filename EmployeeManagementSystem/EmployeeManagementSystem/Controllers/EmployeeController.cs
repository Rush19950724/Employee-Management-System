using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(EmployeeContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var employeeList = _context.Employees.ToList();
            return new JsonResult(employeeList);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            var dbEntry = _context.Employees.Any(x => x.EmployeeName == employee.EmployeeName);
            if (!dbEntry)
            {
                var result = _context.Employees.Add(employee);
                _context.SaveChanges();
                return new JsonResult("Employee added Successfully!");
            }
            return new JsonResult("Record Exists");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            var dbEntry = _context.Employees.Find(employee.EmployeeId); ;
            if (dbEntry != null)
            {

                dbEntry.EmployeeName = employee.EmployeeName;
                _context.SaveChanges();
                return new JsonResult("Employee Updated Successfully!");
            }
            return new JsonResult("There are no any changes!");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int Id)
        {
            var dbEntry = _context.Employees.FirstOrDefault(x => x.EmployeeId == Id); ;
            if (dbEntry != null)
            {
                _context.Remove(dbEntry);
                _context.SaveChanges();
                return new JsonResult("Employee Deleted Successfully!");
            }
            return new JsonResult("Unsuccessful! Try Again");
        }
        
        [Route("SaveAvatar")]
        [HttpPost]
        public JsonResult SaveAvatar()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedImage = httpRequest.Files[0];
                string imageName = postedImage.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/EmployeeAvatars/" + imageName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedImage.CopyTo(stream);
                }

                return new JsonResult(imageName);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames()
        {
            var departmentList = _context.Departments.ToList();
            return new JsonResult(departmentList);
        }

    }
}