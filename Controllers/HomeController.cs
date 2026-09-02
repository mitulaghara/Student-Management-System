using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public HomeController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult Index()
        {
            long studentCount = 0;
            long staffCount = 0;
            long departmentCount = 0;
            long courseCount = 0;
            long classroomCount = 0;
            long enrollmentCount = 0;
            long attendanceCount = 0;
            long markCount = 0;
            long noticeCount = 0;
            long timetableCount = 0;

            long presentCount = 0;
            long absentCount = 0;
            long lateCount = 0;

            List<Student> recentStudents = new();
            List<Notice> recentNotices = new();
            List<DepartmentChartItem> deptChartData = new();

            try
            {
                studentCount = _mongoDbService.Students.CountDocuments(FilterDefinition<Student>.Empty);
                staffCount = _mongoDbService.Staffs.CountDocuments(FilterDefinition<Staff>.Empty);
                departmentCount = _mongoDbService.Departments.CountDocuments(FilterDefinition<Department>.Empty);
                courseCount = _mongoDbService.Courses.CountDocuments(FilterDefinition<Course>.Empty);
                classroomCount = _mongoDbService.Classrooms.CountDocuments(FilterDefinition<Classroom>.Empty);
                enrollmentCount = _mongoDbService.Enrollments.CountDocuments(FilterDefinition<Enrollment>.Empty);
                attendanceCount = _mongoDbService.Attendances.CountDocuments(FilterDefinition<Attendance>.Empty);
                markCount = _mongoDbService.Marks.CountDocuments(FilterDefinition<Mark>.Empty);
                noticeCount = _mongoDbService.Notices.CountDocuments(FilterDefinition<Notice>.Empty);
                timetableCount = _mongoDbService.Timetables.CountDocuments(FilterDefinition<Timetable>.Empty);

                presentCount = _mongoDbService.Attendances.CountDocuments(a => a.Status == "Present");
                absentCount = _mongoDbService.Attendances.CountDocuments(a => a.Status == "Absent");
                lateCount = _mongoDbService.Attendances.CountDocuments(a => a.Status == "Late");

                recentStudents = _mongoDbService.Students
                    .Find(FilterDefinition<Student>.Empty)
                    .SortByDescending(s => s.StudentID)
                    .Limit(5)
                    .ToList();

                recentNotices = _mongoDbService.Notices
                    .Find(n => n.IsActive)
                    .SortByDescending(n => n.PublishedDate)
                    .Limit(4)
                    .ToList();

                var depts = _mongoDbService.Departments.Find(FilterDefinition<Department>.Empty).ToList();
                foreach (var dept in depts)
                {
                    long count = _mongoDbService.Students.CountDocuments(s => s.DepartmentName == dept.DepartmentName);
                    deptChartData.Add(new DepartmentChartItem
                    {
                        DepartmentName = dept.DepartmentName ?? "",
                        StudentCount = count
                    });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Database connection notice: " + ex.Message;
            }

            ViewBag.StudentCount = studentCount;
            ViewBag.StaffCount = staffCount;
            ViewBag.DepartmentCount = departmentCount;
            ViewBag.CourseCount = courseCount;
            ViewBag.ClassroomCount = classroomCount;
            ViewBag.EnrollmentCount = enrollmentCount;
            ViewBag.AttendanceCount = attendanceCount;
            ViewBag.MarkCount = markCount;
            ViewBag.NoticeCount = noticeCount;
            ViewBag.TimetableCount = timetableCount;

            ViewBag.PresentCount = presentCount;
            ViewBag.AbsentCount = absentCount;
            ViewBag.LateCount = lateCount;

            ViewBag.RecentStudents = recentStudents;
            ViewBag.RecentNotices = recentNotices;
            ViewBag.DeptChartData = deptChartData;

            return View();
        }
    }

    public class DepartmentChartItem
    {
        public string DepartmentName { get; set; } = string.Empty;
        public long StudentCount { get; set; }
    }
}
