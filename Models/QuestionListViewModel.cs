namespace UngDungOnThiBangLai.Models
{
    public class QuestionListViewModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string CategoryName { get; set; } // Hiển thị "A1", "B2"...
        public bool IsCritical { get; set; }
        public string QuestionType { get; set; }
        public string? ImageUrl { get; set; }
    }
}