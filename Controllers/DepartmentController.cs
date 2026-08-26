using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class DepartmentController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public DepartmentController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult Index()
        {
            List<Department> departmentList = new();
            bool isMock = false;

            try
            {
                departmentList = _mongoDbService.Departments
                    .Find(FilterDefinition<Department>.Empty)
                    .SortBy(d => d.DepartmentName)
                    .ToList();
            }
            catch (Exception)
            {
                departmentList = new List<Department>
                {
                    new Department { DepartmentID = 1, DepartmentName = "Computer Science", Created = DateTime.Now, Modified = DateTime.Now },
                    new Department { DepartmentID = 2, DepartmentName = "Information Technology", Created = DateTime.Now, Modified = DateTime.Now },
                    new Department { DepartmentID = 3, DepartmentName = "Mechanical Engineering", Created = DateTime.Now, Modified = DateTime.Now }
                };
                isMock = true;
            }

            ViewBag.IsMock = isMock;
            return View("DepartmentList", departmentList);
        }

        [HttpGet]
        public IActionResult DepartmentAddEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Department());
            }

            try
            {
                var department = _mongoDbService.Departments.Find(d => d.DepartmentID == id).FirstOrDefault();
                if (department != null)
                {
                    return View(department);
                }
            }
            catch (Exception) { }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Department model)
        {
            ModelState.Remove("DepartmentID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                return View("DepartmentAddEdit", model);
            }

            try
            {
                if (model.DepartmentID == 0)
                {
                    var last = _mongoDbService.Departments.Find(FilterDefinition<Department>.Empty)
                        .SortByDescending(d => d.DepartmentID)
                        .FirstOrDefault();
                    model.DepartmentID = (last?.DepartmentID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Departments.InsertOne(model);
                    TempData["SuccessMessage"] = "Department added successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Departments.ReplaceOne(d => d.DepartmentID == model.DepartmentID, model);
                    TempData["SuccessMessage"] = "Department updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Departments.DeleteOne(d => d.DepartmentID == id);
            }
            catch (Exception) { }

            return RedirectToAction("Index");
        }
    }
}
