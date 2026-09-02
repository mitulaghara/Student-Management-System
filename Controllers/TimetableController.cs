using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class TimetableController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public TimetableController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult TimetableList()
        {
            List<Timetable> timetableList = new();

            try
            {
                timetableList = _mongoDbService.Timetables
                    .Find(FilterDefinition<Timetable>.Empty)
                    .SortBy(t => t.DayOfWeek)
                    .ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error connecting to Database: " + ex.Message;
            }

            return View(timetableList);
        }

        [HttpGet]
        public IActionResult TimetableAddEdit(int? id)
        {
            PopulateDropdowns();

            if (id == null || id == 0)
            {
                return View(new Timetable { DayOfWeek = "Monday", StartTime = "09:00 AM", EndTime = "10:30 AM" });
            }

            try
            {
                var timetable = _mongoDbService.Timetables.Find(t => t.TimetableID == id).FirstOrDefault();
                if (timetable != null)
                {
                    return View(timetable);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching timetable: " + ex.Message;
            }

            return RedirectToAction("TimetableList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Timetable model)
        {
            ModelState.Remove("TimetableID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("TimetableAddEdit", model);
            }

            try
            {
                if (model.TimetableID == 0)
                {
                    var last = _mongoDbService.Timetables
                        .Find(FilterDefinition<Timetable>.Empty)
                        .SortByDescending(t => t.TimetableID)
                        .FirstOrDefault();
                    model.TimetableID = (last?.TimetableID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Timetables.InsertOne(model);
                    TempData["SuccessMessage"] = "Class schedule added successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Timetables.ReplaceOne(t => t.TimetableID == model.TimetableID, model);
                    TempData["SuccessMessage"] = "Class schedule updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving timetable: " + ex.Message;
            }

            return RedirectToAction("TimetableList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Timetables.DeleteOne(t => t.TimetableID == id);
                TempData["SuccessMessage"] = "Class schedule deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting timetable: " + ex.Message;
            }

            return RedirectToAction("TimetableList");
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

                ViewBag.Staffs = _mongoDbService.Staffs
                    .Find(FilterDefinition<Staff>.Empty)
                    .Project(s => s.StaffName)
                    .ToList();
            }
            catch (Exception)
            {
                ViewBag.Departments = new List<string>();
                ViewBag.Courses = new List<string>();
                ViewBag.Classrooms = new List<string>();
                ViewBag.Staffs = new List<string>();
            }
        }
    }
}
