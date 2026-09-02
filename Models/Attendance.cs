using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Attendance
    {
        [BsonId]
        public int AttendanceID { get; set; }

        [Required(ErrorMessage = "Please select a Student")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Student")]
        public int StudentID { get; set; }

        public string? StudentName { get; set; }

        [Required(ErrorMessage = "Attendance Date is required")]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [Required(ErrorMessage = "Please select Attendance Status")]
        public string? Status { get; set; } // Present, Absent, Late

        public string? Subject { get; set; }

        public string? Remarks { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
