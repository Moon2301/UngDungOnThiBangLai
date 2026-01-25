namespace UngDungOnThiBangLai.Models
{
    public class LicenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } // A1, A2, B1, B2...
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<Question> Questions { get; set; }
    }
    public class Question
    {
        public int Id { get; set; }
        public int LicenseCategoryId { get; set; }
        public string QuestionText { get; set; }
        public string? ImageUrl { get; set; } // Có thể null nếu không có ảnh
        public string? Explanation { get; set; }
        public bool IsCritical { get; set; } // Câu hỏi điểm liệt
        public string QuestionType { get; set; } // "MultipleChoice" hoặc "FillIn"

        // Navigation properties
        public virtual LicenseCategory LicenseCategory { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "User"; // Admin hoặc User
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int Creadit { get; set; } = 0; // Số điểm tín dụng của người dùng
    }

}
