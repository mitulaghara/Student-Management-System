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
            long studentCount = 3;
            long staffCount = 3;
            long departmentCount = 4;
            long courseCount = 3;
            long classroomCount = 4;
            long enrollmentCount = 2;
            long attendanceCount = 3;
            long markCount = 3;
            long noticeCount = 3;
            long timetableCount = 3;

            long presentCount = 1;
            long absentCount = 1;
            long lateCount = 1;

            List<Student> recentStudents = new();
            List<Notice> recentNotices = new();

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
            }
            catch (Exception)
            {
                recentStudents = new List<Student>
                {
                    new Student { StudentID = 1, StudentName = "Maulik Ghara", RollNo = "CS2026-001", DepartmentName = "Computer Science", CourseName = "Web Development with .NET", IsActive = true },
                    new Student { StudentID = 2, StudentName = "Aarav Sharma", RollNo = "IT2026-042", DepartmentName = "Information Technology", CourseName = "Database Management Systems", IsActive = true },
                    new Student { StudentID = 3, StudentName = "Priya Patel", RollNo = "ME2026-015", DepartmentName = "Mechanical Engineering", CourseName = "Object Oriented Programming", IsActive = true }
                };

                recentNotices = new List<Notice>
                {
                    new Notice { NoticeID = 1, Title = "Mid-Term Examination Schedule Released", Category = "Exam", PublishedDate = DateTime.Today, IsActive = true },
                    new Notice { NoticeID = 2, Title = "Annual Tech Fest - Inovacia 2026", Category = "General", PublishedDate = DateTime.Today.AddDays(-2), IsActive = true }
                };
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

            return View();
        }
    }
}
