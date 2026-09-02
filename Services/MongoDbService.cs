using MongoDB.Driver;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration.GetConnectionString("DatabaseName") ?? "SMS";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Department> Departments => _database.GetCollection<Department>("Departments");
        public IMongoCollection<Staff> Staffs => _database.GetCollection<Staff>("Staffs");
        public IMongoCollection<Classroom> Classrooms => _database.GetCollection<Classroom>("Classrooms");
        public IMongoCollection<Course> Courses => _database.GetCollection<Course>("Courses");
        public IMongoCollection<Student> Students => _database.GetCollection<Student>("Students");
        public IMongoCollection<Enrollment> Enrollments => _database.GetCollection<Enrollment>("Enrollments");
        public IMongoCollection<UserLoginModel> Users => _database.GetCollection<UserLoginModel>("Users");
        public IMongoCollection<Attendance> Attendances => _database.GetCollection<Attendance>("Attendances");
        public IMongoCollection<Mark> Marks => _database.GetCollection<Mark>("Marks");

        public void SeedInitialData()
        {
            try
            {
                // Seed Admin User
                if (Users.CountDocuments(FilterDefinition<UserLoginModel>.Empty) == 0)
                {
                    Users.InsertOne(new UserLoginModel { UserName = "admin", Password = "admin123" });
                }

                // Seed Departments
                if (Departments.CountDocuments(FilterDefinition<Department>.Empty) == 0)
                {
                    Departments.InsertMany(new[]
                    {
                        new Department { DepartmentID = 1, DepartmentName = "Computer Science", Created = DateTime.Now, Modified = DateTime.Now },
                        new Department { DepartmentID = 2, DepartmentName = "Information Technology", Created = DateTime.Now, Modified = DateTime.Now },
                        new Department { DepartmentID = 3, DepartmentName = "Mechanical Engineering", Created = DateTime.Now, Modified = DateTime.Now },
                        new Department { DepartmentID = 4, DepartmentName = "Civil Engineering", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Staffs
                if (Staffs.CountDocuments(FilterDefinition<Staff>.Empty) == 0)
                {
                    Staffs.InsertMany(new[]
                    {
                        new Staff { StaffID = 1, StaffName = "Dr. Ramesh Patel", DepartmentName = "Computer Science", MobileNo = "9876543210", EmailAddress = "ramesh.patel@school.edu", Remarks = "Senior Professor", Created = DateTime.Now, Modified = DateTime.Now },
                        new Staff { StaffID = 2, StaffName = "Prof. Sneha Shah", DepartmentName = "Information Technology", MobileNo = "9823456789", EmailAddress = "sneha.shah@school.edu", Remarks = "Associate Professor", Created = DateTime.Now, Modified = DateTime.Now },
                        new Staff { StaffID = 3, StaffName = "Dr. Anil Mehta", DepartmentName = "Mechanical Engineering", MobileNo = "9123456780", EmailAddress = "anil.mehta@school.edu", Remarks = "HOD Mechanical Department", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Classrooms
                if (Classrooms.CountDocuments(FilterDefinition<Classroom>.Empty) == 0)
                {
                    Classrooms.InsertMany(new[]
                    {
                        new Classroom { ClassroomID = 1, ClassroomName = "Lab 1 - Ground Floor", Created = DateTime.Now, Modified = DateTime.Now },
                        new Classroom { ClassroomID = 2, ClassroomName = "Lab 2 - First Floor", Created = DateTime.Now, Modified = DateTime.Now },
                        new Classroom { ClassroomID = 3, ClassroomName = "Seminar Hall 3", Created = DateTime.Now, Modified = DateTime.Now },
                        new Classroom { ClassroomID = 4, ClassroomName = "Classroom 101", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Courses
                if (Courses.CountDocuments(FilterDefinition<Course>.Empty) == 0)
                {
                    Courses.InsertMany(new[]
                    {
                        new Course { CourseID = 1, CourseName = "Web Development with .NET", Remarks = "Full Stack C# MVC Course", Created = DateTime.Now, Modified = DateTime.Now },
                        new Course { CourseID = 2, CourseName = "Database Management Systems", Remarks = "MongoDB & Relational Design", Created = DateTime.Now, Modified = DateTime.Now },
                        new Course { CourseID = 3, CourseName = "Object Oriented Programming", Remarks = "C++ and Java basics", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Students
                if (Students.CountDocuments(FilterDefinition<Student>.Empty) == 0)
                {
                    Students.InsertMany(new[]
                    {
                        new Student { StudentID = 1, StudentName = "Maulik Ghara", RollNo = "CS2026-001", EmailAddress = "maulik.ghara@student.edu", MobileNo = "9988776655", BirthDate = new DateTime(2004, 5, 15), DepartmentName = "Computer Science", CourseName = "Web Development with .NET", ClassroomName = "Lab 1 - Ground Floor", IsActive = true, Created = DateTime.Now, Modified = DateTime.Now },
                        new Student { StudentID = 2, StudentName = "Aarav Sharma", RollNo = "IT2026-042", EmailAddress = "aarav.sharma@student.edu", MobileNo = "9898989898", BirthDate = new DateTime(2004, 9, 22), DepartmentName = "Information Technology", CourseName = "Database Management Systems", ClassroomName = "Lab 2 - First Floor", IsActive = true, Created = DateTime.Now, Modified = DateTime.Now },
                        new Student { StudentID = 3, StudentName = "Priya Patel", RollNo = "ME2026-015", EmailAddress = "priya.patel@student.edu", MobileNo = "9797979797", BirthDate = new DateTime(2005, 1, 10), DepartmentName = "Mechanical Engineering", CourseName = "Object Oriented Programming", ClassroomName = "Classroom 101", IsActive = true, Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Enrollments
                if (Enrollments.CountDocuments(FilterDefinition<Enrollment>.Empty) == 0)
                {
                    Enrollments.InsertMany(new[]
                    {
                        new Enrollment { EnrollmentID = 1, StudentID = 1, StudentName = "Maulik Ghara", StaffID = 1, StaffName = "Dr. Ramesh Patel", IsActive = true, Remarks = "Assigned to Senior Advisor", Created = DateTime.Now, Modified = DateTime.Now },
                        new Enrollment { EnrollmentID = 2, StudentID = 2, StudentName = "Aarav Sharma", StaffID = 2, StaffName = "Prof. Sneha Shah", IsActive = true, Remarks = "Assigned to Advisor Sneha", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Attendances
                if (Attendances.CountDocuments(FilterDefinition<Attendance>.Empty) == 0)
                {
                    Attendances.InsertMany(new[]
                    {
                        new Attendance { AttendanceID = 1, StudentID = 1, StudentName = "Maulik Ghara", AttendanceDate = DateTime.Today, Status = "Present", Subject = "Web Development with .NET", Remarks = "", Created = DateTime.Now, Modified = DateTime.Now },
                        new Attendance { AttendanceID = 2, StudentID = 2, StudentName = "Aarav Sharma", AttendanceDate = DateTime.Today, Status = "Absent", Subject = "Database Management Systems", Remarks = "Medical Leave", Created = DateTime.Now, Modified = DateTime.Now },
                        new Attendance { AttendanceID = 3, StudentID = 3, StudentName = "Priya Patel", AttendanceDate = DateTime.Today, Status = "Late", Subject = "Object Oriented Programming", Remarks = "Traffic delay", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }

                // Seed Marks
                if (Marks.CountDocuments(FilterDefinition<Mark>.Empty) == 0)
                {
                    Marks.InsertMany(new[]
                    {
                        new Mark { MarkID = 1, StudentID = 1, StudentName = "Maulik Ghara", CourseName = "Web Development with .NET", ExamType = "Mid-Term", MarksObtained = 88, TotalMarks = 100, Grade = "A+", Remarks = "Excellent performance", Created = DateTime.Now, Modified = DateTime.Now },
                        new Mark { MarkID = 2, StudentID = 2, StudentName = "Aarav Sharma", CourseName = "Database Management Systems", ExamType = "Practical", MarksObtained = 76, TotalMarks = 100, Grade = "B+", Remarks = "Good practical skills", Created = DateTime.Now, Modified = DateTime.Now },
                        new Mark { MarkID = 3, StudentID = 3, StudentName = "Priya Patel", CourseName = "Object Oriented Programming", ExamType = "Final", MarksObtained = 92, TotalMarks = 100, Grade = "A+", Remarks = "Outstanding results", Created = DateTime.Now, Modified = DateTime.Now }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MongoDB Seed Error: " + ex.Message);
            }
        }
    }
}
