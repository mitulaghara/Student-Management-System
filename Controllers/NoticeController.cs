using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class NoticeController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public NoticeController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult NoticeList()
        {
            List<Notice> noticeList = new();
            bool isMock = false;

            try
            {
                noticeList = _mongoDbService.Notices
                    .Find(FilterDefinition<Notice>.Empty)
                    .SortByDescending(n => n.PublishedDate)
                    .ToList();
            }
            catch (Exception)
            {
                noticeList = new List<Notice>
                {
                    new Notice { NoticeID = 1, Title = "Mid-Term Examination Schedule Released", Content = "The mid-term examination timetable for Semester 5 has been published. Check your respective department notices.", Category = "Exam", PublishedDate = DateTime.Today, IsActive = true, Created = DateTime.Now, Modified = DateTime.Now },
                    new Notice { NoticeID = 2, Title = "Annual Tech Fest - Inovacia 2026", Content = "Registration is now open for hackathons, robotics, and coding competitions.", Category = "General", PublishedDate = DateTime.Today.AddDays(-2), IsActive = true, Created = DateTime.Now, Modified = DateTime.Now },
                    new Notice { NoticeID = 3, Title = "Independence Day Holiday", Content = "The university will remain closed on the occasion of Independence Day.", Category = "Holiday", PublishedDate = DateTime.Today.AddDays(-10), IsActive = false, Created = DateTime.Now, Modified = DateTime.Now }
                };
                isMock = true;
            }

            ViewBag.IsMock = isMock;
            return View(noticeList);
        }

        [HttpGet]
        public IActionResult NoticeAddEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Notice { PublishedDate = DateTime.Today, IsActive = true, Category = "Academic" });
            }

            try
            {
                var notice = _mongoDbService.Notices.Find(n => n.NoticeID == id).FirstOrDefault();
                if (notice != null)
                {
                    return View(notice);
                }
            }
            catch (Exception) { }

            return RedirectToAction("NoticeList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Notice model)
        {
            ModelState.Remove("NoticeID");
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                return View("NoticeAddEdit", model);
            }

            try
            {
                if (model.NoticeID == 0)
                {
                    var last = _mongoDbService.Notices
                        .Find(FilterDefinition<Notice>.Empty)
                        .SortByDescending(n => n.NoticeID)
                        .FirstOrDefault();
                    model.NoticeID = (last?.NoticeID ?? 0) + 1;
                    model.Created = DateTime.Now;
                    model.Modified = DateTime.Now;

                    _mongoDbService.Notices.InsertOne(model);
                    TempData["SuccessMessage"] = "Notice published successfully!";
                }
                else
                {
                    model.Modified = DateTime.Now;
                    _mongoDbService.Notices.ReplaceOne(n => n.NoticeID == model.NoticeID, model);
                    TempData["SuccessMessage"] = "Notice updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving notice: " + ex.Message;
            }

            return RedirectToAction("NoticeList");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _mongoDbService.Notices.DeleteOne(n => n.NoticeID == id);
                TempData["SuccessMessage"] = "Notice deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting notice: " + ex.Message;
            }

            return RedirectToAction("NoticeList");
        }
    }
}
