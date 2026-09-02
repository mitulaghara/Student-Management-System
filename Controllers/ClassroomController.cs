using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class ClassroomController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public ClassroomController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult ClassroomList()
        {
            List<Classroom> classroomList = new();

            try
            {
                classroomList = _mongoDbService.Classrooms
                    .Find(FilterDefinition<Classroom>.Empty)
                    .SortBy(c => c.ClassroomName)
                    .ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error connecting to Database: " + ex.Message;
            }

            return View(classroomList);
        }

        [HttpGet]
        public IActionResult ClassroomAddEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Classroom());
            }

            try
            {
                var classroom = _mongoDbService.Classrooms.Find(c => c.ClassroomID == id).FirstOrDefault();
                if (classroom != null)
                {
                    return View(classroom);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching classroom: " + ex.Message;
            }

            return RedirectToAction("ClassroomList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Classroom model)
        {
            ModelState.Remove("ClassroomID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                return View("ClassroomAddEdit", model);
            }

            try
            {
                if (model.ClassroomID == 0)
                {
                    var last = _mongoDbService.Classrooms.Find(FilterDefinition<Classroom>.Empty)
                        .SortByDescending(c => c.ClassroomID)
                        .FirstOrDefault();
                    model.ClassroomID = (last?.ClassroomID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Classrooms.InsertOne(model);
                    TempData["SuccessMessage"] = "Classroom added successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Classrooms.ReplaceOne(c => c.ClassroomID == model.ClassroomID, model);
                    TempData["SuccessMessage"] = "Classroom updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return RedirectToAction("ClassroomList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Classrooms.DeleteOne(c => c.ClassroomID == id);
                TempData["SuccessMessage"] = "Classroom deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting classroom: " + ex.Message;
            }

            return RedirectToAction("ClassroomList");
        }
    }
}
