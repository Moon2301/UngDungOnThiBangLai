using Microsoft.AspNetCore.Http;

namespace UngDungOnThiBangLai.Models
{
    public class TrafficSignViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }         // P.102
        public string Name { get; set; }         // Cấm đi ngược chiều
        public string Category { get; set; }     // Biển cấm, Biển báo nguy hiểm...
        public string Description { get; set; }  // Ý nghĩa chi tiết
        public IFormFile? ImageFile { get; set; } // File ảnh upload
        public string? ExistingImageUrl { get; set; } // Dùng cho trang Edit
    }
}