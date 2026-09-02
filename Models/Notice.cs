using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Notice
    {
        [BsonId]
        public int NoticeID { get; set; }

        [Required(ErrorMessage = "Notice Title is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Notice Content is required")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string? Category { get; set; } // Academic, Exam, Holiday, General, Sports

        [Required(ErrorMessage = "Published Date is required")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
