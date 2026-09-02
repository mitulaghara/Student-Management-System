using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class CourseController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public CourseController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult CourseList()
        {
            List<Course> courseList = new();

            try
            {
                courseList = _mongoDbService.Courses
                    .Find(FilterDefinition<Course>.Empty)
                    .SortBy(c => c.CourseName)
                    .ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error connecting to Database: " + ex.Message;
            }

            return View(courseList);
        }

        [HttpGet]
        public IActionResult CourseAddEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Course());
            }

            try
            {
                var course = _mongoDbService.Courses.Find(c => c.CourseID == id).FirstOrDefault();
                if (course != null)
                {
                    return View(course);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching course: " + ex.Message;
            }

            return RedirectToAction("CourseList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Course model)
        {
            ModelState.Remove("CourseID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                return View("CourseAddEdit", model);
            }

            try
            {
                if (model.CourseID == 0)
                {
                    var last = _mongoDbService.Courses.Find(FilterDefinition<Course>.Empty)
                        .SortByDescending(c => c.CourseID)
                        .FirstOrDefault();
                    model.CourseID = (last?.CourseID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Courses.InsertOne(model);
                    TempData["SuccessMessage"] = "Course added successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Courses.ReplaceOne(c => c.CourseID == model.CourseID, model);
                    TempData["SuccessMessage"] = "Course updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return RedirectToAction("CourseList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Courses.DeleteOne(c => c.CourseID == id);
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting course: " + ex.Message;
            }

            return RedirectToAction("CourseList");
        }
    }
}
