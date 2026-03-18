namespace UngDungOnThiBangLai.Models
{
    public class QuestionListViewModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsCritical { get; set; }
        public string CategoryName { get; set; }
        public string QuestionType { get; set; }
        public string TopicName { get; set; }
    }
}