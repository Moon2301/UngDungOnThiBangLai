using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UngDungOnThiBangLai.Models
{
    public class LicenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } // A1, A2, B1, B2...
        public string? Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        public int LicenseCategoryId { get; set; }

        [Required]
        public string QuestionText { get; set; }

        // Ảnh tự do (như sa hình, tình huống)
        public string? ImageUrl { get; set; }

        // LIÊN KẾT BIỂN BÁO: Nếu câu hỏi dùng biển báo có sẵn
        public int? TrafficSignId { get; set; }
        public virtual TrafficSign? TrafficSign { get; set; }

        public string? Explanation { get; set; }
        public bool IsCritical { get; set; }
        public string QuestionType { get; set; } // "MultipleChoice" hoặc "FillIn"

        public virtual LicenseCategory LicenseCategory { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }

        // Đáp án có thể là chữ hoặc để trống nếu chỉ dùng ảnh biển báo
        public string? AnswerText { get; set; }

        // Ảnh tự do cho đáp án
        public string? ImageUrl { get; set; }

        // LIÊN KẾT BIỂN BÁO: Nếu đáp án là một hình biển báo trong thư viện
        public int? TrafficSignId { get; set; }
        public virtual TrafficSign? TrafficSign { get; set; }

        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; }
    }

    public class TrafficSign
    {
        public int Id { get; set; }
        public string Code { get; set; }         // Ví dụ: "P.102"
        public string Name { get; set; }         // Ví dụ: "Cấm đi ngược chiều"
        public string ImageUrl { get; set; }     // Đường dẫn ảnh biển báo gốc
        public string Category { get; set; }     // "Cấm", "Nguy hiểm", "Hiệu lệnh"...
        public string Description { get; set; }  // Ý nghĩa chi tiết biển báo

        // Thuộc tính này giúp tra cứu xem biển báo này xuất hiện ở những câu hỏi nào
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Credit { get; set; } = 0;
    }
}
