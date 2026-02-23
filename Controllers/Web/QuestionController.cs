using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UngDungOnThiBangLai.Models;

namespace UngDungOnThiBangLai.Controllers.Web
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách câu hỏi
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var questionsQuery = _context.Questions
                .Include(q => q.LicenseCategory)
                .AsQueryable();

            // Bộ lọc tìm kiếm theo nội dung câu hỏi
            if (!string.IsNullOrEmpty(searchString))
            {
                questionsQuery = questionsQuery.Where(s => s.QuestionText.Contains(searchString));
            }

            // Bộ lọc theo hạng bằng lái
            if (categoryId.HasValue)
            {
                questionsQuery = questionsQuery.Where(q => q.LicenseCategoryId == categoryId);
            }

            var result = await questionsQuery.Select(q => new QuestionListViewModel
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                CategoryName = q.LicenseCategory.Name,
                IsCritical = q.IsCritical,
                QuestionType = q.QuestionType,
                ImageUrl = q.ImageUrl
            }).ToListAsync();

            ViewBag.Categories = await _context.LicenseCategories.ToListAsync();
            return View(result);
        }
    }
}