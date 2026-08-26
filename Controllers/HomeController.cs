using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public HomeController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult Index()
        {
            long studentCount = 3;
            long staffCount = 3;
            long departmentCount = 4;
            long courseCount = 3;
            long classroomCount = 4;
            long enrollmentCount = 2;

            try
            {
                studentCount = _mongoDbService.Students.CountDocuments(FilterDefinition<Student>.Empty);
                staffCount = _mongoDbService.Staffs.CountDocuments(FilterDefinition<Staff>.Empty);
                departmentCount = _mongoDbService.Departments.CountDocuments(FilterDefinition<Department>.Empty);
                courseCount = _mongoDbService.Courses.CountDocuments(FilterDefinition<Course>.Empty);
                classroomCount = _mongoDbService.Classrooms.CountDocuments(FilterDefinition<Classroom>.Empty);
                enrollmentCount = _mongoDbService.Enrollments.CountDocuments(FilterDefinition<Enrollment>.Empty);
            }
            catch (Exception) { }

            ViewBag.StudentCount = studentCount;
            ViewBag.StaffCount = staffCount;
            ViewBag.DepartmentCount = departmentCount;
            ViewBag.CourseCount = courseCount;
            ViewBag.ClassroomCount = classroomCount;
            ViewBag.EnrollmentCount = enrollmentCount;

            return View();
        }
    }
}
