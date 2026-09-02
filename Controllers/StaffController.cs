using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class StaffController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public StaffController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult StaffList()
        {
            List<Staff> staffList = new();

            try
            {
                staffList = _mongoDbService.Staffs
                    .Find(FilterDefinition<Staff>.Empty)
                    .SortBy(s => s.StaffName)
                    .ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error connecting to Database: " + ex.Message;
            }

            return View(staffList);
        }

        [HttpGet]
        public IActionResult StaffAddEdit(int? id)
        {
            ViewBag.Departments = GetDepartmentNames();

            if (id == null || id == 0)
            {
                return View(new Staff());
            }

            try
            {
                var staff = _mongoDbService.Staffs.Find(s => s.StaffID == id).FirstOrDefault();
                if (staff != null)
                {
                    return View(staff);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching staff: " + ex.Message;
            }

            return RedirectToAction("StaffList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Staff model)
        {
            ModelState.Remove("StaffID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = GetDepartmentNames();
                return View("StaffAddEdit", model);
            }

            try
            {
                if (model.StaffID == 0)
                {
                    var last = _mongoDbService.Staffs.Find(FilterDefinition<Staff>.Empty)
                        .SortByDescending(s => s.StaffID)
                        .FirstOrDefault();
                    model.StaffID = (last?.StaffID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Staffs.InsertOne(model);
                    TempData["SuccessMessage"] = "Staff member added successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Staffs.ReplaceOne(s => s.StaffID == model.StaffID, model);
                    TempData["SuccessMessage"] = "Staff member updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return RedirectToAction("StaffList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Staffs.DeleteOne(s => s.StaffID == id);
                TempData["SuccessMessage"] = "Staff member deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting staff: " + ex.Message;
            }

            return RedirectToAction("StaffList");
        }

        private List<string?> GetDepartmentNames()
        {
            try
            {
                return _mongoDbService.Departments
                    .Find(FilterDefinition<Department>.Empty)
                    .Project(d => d.DepartmentName)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<string?>();
            }
        }
    }
}
