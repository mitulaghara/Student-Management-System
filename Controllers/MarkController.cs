using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class MarkController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public MarkController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult MarkList()
        {
            List<Mark> markList = new();
            bool isMock = false;

            try
            {
                markList = _mongoDbService.Marks
                    .Find(FilterDefinition<Mark>.Empty)
                    .SortByDescending(m => m.MarkID)
                    .ToList();
            }
            catch (Exception)
            {
                markList = new List<Mark>
                {
                    new Mark { MarkID = 1, StudentID = 1, StudentName = "Maulik Ghara", CourseName = "Web Development with .NET", ExamType = "Mid-Term", MarksObtained = 88, TotalMarks = 100, Grade = "A+", Remarks = "Excellent performance", Created = DateTime.Now, Modified = DateTime.Now },
                    new Mark { MarkID = 2, StudentID = 2, StudentName = "Aarav Sharma", CourseName = "Database Management Systems", ExamType = "Practical", MarksObtained = 76, TotalMarks = 100, Grade = "B+", Remarks = "Good practical skills", Created = DateTime.Now, Modified = DateTime.Now },
                    new Mark { MarkID = 3, StudentID = 3, StudentName = "Priya Patel", CourseName = "Object Oriented Programming", ExamType = "Final", MarksObtained = 92, TotalMarks = 100, Grade = "A+", Remarks = "Outstanding results", Created = DateTime.Now, Modified = DateTime.Now }
                };
                isMock = true;
            }

            ViewBag.IsMock = isMock;
            return View(markList);
        }

        [HttpGet]
        public IActionResult MarkAddEdit(int? id)
        {
            PopulateDropdowns();

            if (id == null || id == 0)
            {
                return View(new Mark { TotalMarks = 100, ExamType = "Mid-Term" });
            }

            try
            {
                var mark = _mongoDbService.Marks.Find(m => m.MarkID == id).FirstOrDefault();
                if (mark != null)
                {
                    return View(mark);
                }
            }
            catch (Exception) { }

            return RedirectToAction("MarkList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Mark model)
        {
            ModelState.Remove("MarkID");
            ModelState.Remove("StudentName");
            ModelState.Remove("Grade");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("MarkAddEdit", model);
            }

            // Calculate Grade based on percentage
            if (model.TotalMarks > 0)
            {
                double percentage = ((double)model.MarksObtained / model.TotalMarks) * 100;
                if (percentage >= 90) model.Grade = "A+";
                else if (percentage >= 80) model.Grade = "A";
                else if (percentage >= 70) model.Grade = "B+";
                else if (percentage >= 60) model.Grade = "B";
                else if (percentage >= 50) model.Grade = "C";
                else if (percentage >= 40) model.Grade = "D";
                else model.Grade = "F";
            }
            else
            {
                model.Grade = "N/A";
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
                if (model.MarkID == 0)
                {
                    var last = _mongoDbService.Marks
                        .Find(FilterDefinition<Mark>.Empty)
                        .SortByDescending(m => m.MarkID)
                        .FirstOrDefault();
                    model.MarkID = (last?.MarkID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Marks.InsertOne(model);
                    TempData["SuccessMessage"] = "Marks recorded successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Marks.ReplaceOne(m => m.MarkID == model.MarkID, model);
                    TempData["SuccessMessage"] = "Marks updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving marks: " + ex.Message;
            }

            return RedirectToAction("MarkList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Marks.DeleteOne(m => m.MarkID == id);
                TempData["SuccessMessage"] = "Mark record deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting mark: " + ex.Message;
            }

            return RedirectToAction("MarkList");
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
