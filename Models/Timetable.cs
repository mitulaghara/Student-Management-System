using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Timetable
    {
        [BsonId]
        public int TimetableID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string? DepartmentName { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Classroom is required")]
        public string? ClassroomName { get; set; }

        [Required(ErrorMessage = "Faculty / Staff is required")]
        public string? StaffName { get; set; }

        [Required(ErrorMessage = "Day of Week is required")]
        public string? DayOfWeek { get; set; } // Monday, Tuesday, Wednesday, Thursday, Friday, Saturday

        [Required(ErrorMessage = "Start Time is required")]
        public string? StartTime { get; set; } // e.g. "09:00 AM"

        [Required(ErrorMessage = "End Time is required")]
        public string? EndTime { get; set; } // e.g. "10:30 AM"

        [Required(ErrorMessage = "Subject Name is required")]
        public string? Subject { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
