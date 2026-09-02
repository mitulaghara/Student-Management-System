using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class StudentController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public StudentController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult StudentList()
        {
            List<Student> studentList = new();

            try
            {
                studentList = _mongoDbService.Students
                    .Find(FilterDefinition<Student>.Empty)
                    .SortBy(s => s.StudentName)
                    .ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error connecting to Database: " + ex.Message;
            }

            return View(studentList);
        }

        [HttpGet]
        public IActionResult StudentAddEdit(int? id)
        {
            PopulateDropdowns();

            if (id == null || id == 0)
            {
                return View(new Student { IsActive = true, BirthDate = DateTime.Today.AddYears(-20) });
            }

            try
            {
                var student = _mongoDbService.Students.Find(s => s.StudentID == id).FirstOrDefault();
                if (student != null)
                {
                    return View(student);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching student: " + ex.Message;
            }

            return RedirectToAction("StudentList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Student model)
        {
            ModelState.Remove("StudentID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("StudentAddEdit", model);
            }

            // Set drop info if student is inactive
            if (!model.IsActive)
            {
                if (model.DropDate == null) model.DropDate = DateTime.Now;
                if (string.IsNullOrWhiteSpace(model.DropReason)) model.DropReason = "Not Specified";
            }
            else
            {
                model.DropDate = null;
                model.DropReason = null;
            }

            try
            {
                if (model.StudentID == 0)
                {
                    var last = _mongoDbService.Students
                        .Find(FilterDefinition<Student>.Empty)
                        .SortByDescending(s => s.StudentID)
                        .FirstOrDefault();
                    model.StudentID = (last?.StudentID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Students.InsertOne(model);
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Students.ReplaceOne(s => s.StudentID == model.StudentID, model);
                }

                TempData["SuccessMessage"] = model.StudentID > 0 && model.Created != model.Modified
                    ? "Student record updated successfully!"
                    : "Student registered successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving student: " + ex.Message;
            }

            return RedirectToAction("StudentList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Students.DeleteOne(s => s.StudentID == id);
                TempData["SuccessMessage"] = "Student record deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting student: " + ex.Message;
            }

            return RedirectToAction("StudentList");
        }

        public IActionResult ExportToCsv()
        {
            List<Student> students = new();
            try
            {
                students = _mongoDbService.Students.Find(FilterDefinition<Student>.Empty).ToList();
            }
            catch (Exception)
            {
                students = new List<Student>();
            }

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("StudentID,RollNo,StudentName,Department,Course,Classroom,MobileNo,EmailAddress,Status");

            foreach (var s in students)
            {
                string status = s.IsActive ? "Active" : "Dropped";
                builder.AppendLine($"\"{s.StudentID}\",\"{s.RollNo}\",\"{s.StudentName}\",\"{s.DepartmentName}\",\"{s.CourseName}\",\"{s.ClassroomName}\",\"{s.MobileNo}\",\"{s.EmailAddress}\",\"{status}\"");
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            return File(buffer, "text/csv", $"Students_Export_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }

        private void PopulateDropdowns()
        {
            try
            {
                ViewBag.Departments = _mongoDbService.Departments
                    .Find(FilterDefinition<Department>.Empty)
                    .Project(d => d.DepartmentName)
                    .ToList();

                ViewBag.Courses = _mongoDbService.Courses
                    .Find(FilterDefinition<Course>.Empty)
                    .Project(c => c.CourseName)
                    .ToList();

                ViewBag.Classrooms = _mongoDbService.Classrooms
                    .Find(FilterDefinition<Classroom>.Empty)
                    .Project(c => c.ClassroomName)
                    .ToList();
            }
            catch (Exception)
            {
                ViewBag.Departments = new List<string>();
                ViewBag.Courses = new List<string>();
                ViewBag.Classrooms = new List<string>();
            }
        }
    }
}
