using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class EnrollmentController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public EnrollmentController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult EnrollmentList()
        {
            List<Enrollment> enrollmentList = new();
            bool isMock = false;

            try
            {
                enrollmentList = _mongoDbService.Enrollments
                    .Find(FilterDefinition<Enrollment>.Empty)
                    .SortBy(e => e.EnrollmentID)
                    .ToList();
            }
            catch (Exception)
            {
                enrollmentList = new List<Enrollment>
                {
                    new Enrollment { EnrollmentID = 1, StudentID = 1, StudentName = "Maulik Ghara", StaffID = 1, StaffName = "Dr. Ramesh Patel", IsActive = true, Remarks = "Assigned to Senior Advisor", Created = DateTime.Now, Modified = DateTime.Now }
                };
                isMock = true;
            }

            ViewBag.IsMock = isMock;
            return View(enrollmentList);
        }

        [HttpGet]
        public IActionResult EnrollmentAddEdit(int? id)
        {
            PopulateDropdowns();

            if (id == null || id == 0)
            {
                return View(new Enrollment { IsActive = true });
            }

            try
            {
                var enrollment = _mongoDbService.Enrollments.Find(e => e.EnrollmentID == id).FirstOrDefault();
                if (enrollment != null)
                {
                    return View(enrollment);
                }
            }
            catch (Exception) { }

            return RedirectToAction("EnrollmentList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Enrollment model)
        {
            ModelState.Remove("EnrollmentID");
            ModelState.Remove("StudentName");
            ModelState.Remove("StaffName");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View("EnrollmentAddEdit", model);
            }

            // Populate Student and Staff Names from DB
            try
            {
                var student = _mongoDbService.Students.Find(s => s.StudentID == model.StudentID).FirstOrDefault();
                var staff = _mongoDbService.Staffs.Find(s => s.StaffID == model.StaffID).FirstOrDefault();
                model.StudentName = student?.StudentName ?? "Student #" + model.StudentID;
                model.StaffName = staff?.StaffName ?? "Staff #" + model.StaffID;
            }
            catch (Exception) { }

            try
            {
                if (model.EnrollmentID == 0)
                {
                    var last = _mongoDbService.Enrollments.Find(FilterDefinition<Enrollment>.Empty)
                        .SortByDescending(e => e.EnrollmentID)
                        .FirstOrDefault();
                    model.EnrollmentID = (last?.EnrollmentID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Enrollments.InsertOne(model);
                    TempData["SuccessMessage"] = "Enrollment added successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Enrollments.ReplaceOne(e => e.EnrollmentID == model.EnrollmentID, model);
                    TempData["SuccessMessage"] = "Enrollment updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return RedirectToAction("EnrollmentList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Enrollments.DeleteOne(e => e.EnrollmentID == id);
            }
            catch (Exception) { }

            return RedirectToAction("EnrollmentList");
        }

        private void PopulateDropdowns()
        {
            try
            {
                ViewBag.Students = _mongoDbService.Students.Find(FilterDefinition<Student>.Empty).ToList();
                ViewBag.Staffs = _mongoDbService.Staffs.Find(FilterDefinition<Staff>.Empty).ToList();
            }
            catch (Exception)
            {
                ViewBag.Students = new List<Student> { new Student { StudentID = 1, StudentName = "Maulik Ghara" } };
                ViewBag.Staffs = new List<Staff> { new Staff { StaffID = 1, StaffName = "Dr. Ramesh Patel" } };
            }
        }
    }
}
