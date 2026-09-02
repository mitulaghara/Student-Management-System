using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Mark
    {
        [BsonId]
        public int MarkID { get; set; }

        [Required(ErrorMessage = "Please select a Student")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Student")]
        public int StudentID { get; set; }

        public string? StudentName { get; set; }

        [Required(ErrorMessage = "Course/Subject is required")]
        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Exam Type is required")]
        public string? ExamType { get; set; } // Internal, External, Practical

        [Required(ErrorMessage = "Marks Obtained is required")]
        [Range(0, 1000, ErrorMessage = "Marks Obtained must be between 0 and 1000")]
        public int MarksObtained { get; set; }

        [Required(ErrorMessage = "Total Marks is required")]
        [Range(1, 1000, ErrorMessage = "Total Marks must be between 1 and 1000")]
        public int TotalMarks { get; set; }

        public string? Grade { get; set; } // Auto-calculated: A+, A, B, C, D, F

        public string? Remarks { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
