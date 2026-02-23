using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UngDungOnThiBangLai.Models
{
    public class CreateQuestionViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập nội dung câu hỏi")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hạng bằng lái")]
        public int LicenseCategoryId { get; set; }

        public string? Explanation { get; set; }

        public bool IsCritical { get; set; }

        [Required]
        public string QuestionType { get; set; } = "MultipleChoice";

        public IFormFile? ImageFile { get; set; } // Dùng để nhận file ảnh từ client
    }
}