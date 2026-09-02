using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class AttendanceController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public AttendanceController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult AttendanceList()
        {
            List<Attendance> attendanceList = new();
            bool isMock = false;

            try
            {
                attendanceList = _mongoDbService.Attendances
                    .Find(FilterDefinition<Attendance>.Empty)
                    .SortByDescending(a => a.AttendanceDate)
                    .ToList();
            }
            catch (Exception)
            {
                attendanceList = new List<Attendance>
                {
                    new Attendance { AttendanceID = 1, StudentID = 1, StudentName = "Maulik Ghara", AttendanceDate = DateTime.Today, Status = "Present", Subject = "Web Development with .NET", Remarks = "", Created = DateTime.Now, Modified = DateTime.Now },
                    new Attendance { AttendanceID = 2, StudentID = 2, StudentName = "Aarav Sharma", AttendanceDate = DateTime.Today, Status = "Absent", Subject = "Database Management Systems", Remarks = "Medical Leave", Created = DateTime.Now, Modified = DateTime.Now }
                };
                isMock = true;
            }

            ViewBag.IsMock = isMock;
            return View(attendanceList);
        }

        [HttpGet]
        public IActionResult AttendanceAddEdit(int? id)
        {
            PopulateDropdowns();

            if (id == null || id == 0)
            {
                return View(new Attendance { AttendanceDate = DateTime.Today, Status = "Present" });
            }

            try
            {
                var attendance = _mongoDbService.Attendances.Find(a => a.AttendanceID == id).FirstOrDefault();
                if (attendance != null)
                {
                    return View(attendance);
                }
            }
            catch (Exception) { }

            return RedirectToAction("AttendanceList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Attendance model)
        {
            ModelState.Remove("AttendanceID");
            ModelState.Remove("StudentName");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("AttendanceAddEdit", model);
            }

            // Populate Student name from DB
            try
            {
                var student = _mongoDbService.Students.Find(s => s.StudentID == model.StudentID).FirstOrDefault();
                model.StudentName = student?.StudentName ?? "Student #" + model.StudentID;
            }
            catch (Exception) { }

            try
            {
                if (model.AttendanceID == 0)
                {
                    var last = _mongoDbService.Attendances
                        .Find(FilterDefinition<Attendance>.Empty)
                        .SortByDescending(a => a.AttendanceID)
                        .FirstOrDefault();
                    model.AttendanceID = (last?.AttendanceID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Attendances.InsertOne(model);
                    TempData["SuccessMessage"] = "Attendance marked successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Attendances.ReplaceOne(a => a.AttendanceID == model.AttendanceID, model);
                    TempData["SuccessMessage"] = "Attendance record updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving attendance: " + ex.Message;
            }

            return RedirectToAction("AttendanceList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Attendances.DeleteOne(a => a.AttendanceID == id);
                TempData["SuccessMessage"] = "Attendance record deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction("AttendanceList");
        }

        public IActionResult ExportToCsv()
        {
            List<Attendance> attendances;
            try
            {
                attendances = _mongoDbService.Attendances.Find(FilterDefinition<Attendance>.Empty).ToList();
            }
            catch (Exception)
            {
                attendances = new List<Attendance>();
            }

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("AttendanceID,StudentID,StudentName,Date,Subject,Status,Remarks");

            foreach (var a in attendances)
            {
                builder.AppendLine($"\"{a.AttendanceID}\",\"{a.StudentID}\",\"{a.StudentName}\",\"{a.AttendanceDate:yyyy-MM-dd}\",\"{a.Subject}\",\"{a.Status}\",\"{a.Remarks}\"");
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            return File(buffer, "text/csv", $"Attendance_Export_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }

        private void PopulateDropdowns()
        {
            try
            {
                ViewBag.Students = _mongoDbService.Students.Find(FilterDefinition<Student>.Empty).ToList();
                ViewBag.Courses = _mongoDbService.Courses
                    .Find(FilterDefinition<Course>.Empty)
                    .Project(c => c.CourseName)
                    .ToList();
            }
            catch (Exception)
            {
                ViewBag.Students = new List<Student>
                {
                    new Student { StudentID = 1, StudentName = "Maulik Ghara" },
                    new Student { StudentID = 2, StudentName = "Aarav Sharma" },
                    new Student { StudentID = 3, StudentName = "Priya Patel" }
                };
                ViewBag.Courses = new List<string> { "Web Development with .NET", "Database Management Systems", "Object Oriented Programming" };
            }
        }
    }
}
