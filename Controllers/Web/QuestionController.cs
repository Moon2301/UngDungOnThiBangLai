using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UngDungOnThiBangLai.Models;

namespace UngDungOnThiBangLai.Controllers.Web
{
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuestionController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Hàm helper dùng chung cho Create và Edit
        private async Task<string?> ProcessUploadedFile(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "questions");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/questions/" + uniqueFileName;
        }

        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var query = _context.Questions.Include(q => q.LicenseCategory).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(q => q.QuestionText.Contains(searchString));

            if (categoryId.HasValue)
                query = query.Where(q => q.LicenseCategoryId == categoryId);

            var result = await query.Select(q => new QuestionListViewModel
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                ImageUrl = q.ImageUrl,
                IsCritical = q.IsCritical,
                CategoryName = q.LicenseCategory.Name,
                QuestionType = q.QuestionType
            }).ToListAsync();

            ViewBag.Categories = await _context.LicenseCategories.ToListAsync();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.LicenseCategories.ToList();
            return View(new CreateQuestionViewModel { Answers = new List<AnswerViewModel> { new AnswerViewModel(), new AnswerViewModel() } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Lưu ảnh câu hỏi (nếu có)
                string? questionImageUrl = await SaveFile(model.ImageFile, "questions");

                var question = new Question
                {
                    LicenseCategoryId = model.LicenseCategoryId,
                    QuestionText = model.QuestionText,
                    Explanation = model.Explanation,
                    IsCritical = model.IsCritical,
                    QuestionType = "MultipleChoice", // Hoặc logic phân loại của bạn
                    ImageUrl = questionImageUrl
                };

                // 2. Lưu danh sách đáp án
                foreach (var a in model.Answers)
                {
                    string? answerImageUrl = await SaveFile(a.ImageFile, "answers");
                    question.Answers.Add(new Answer
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect,
                        ImageUrl = answerImageUrl
                    });
                }

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // --- HELPER METHODS ---
        private async Task<string?> SaveFile(IFormFile? file, string subFolder)
        {
            if (file == null || file.Length == 0) return null;

            string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", subFolder);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/uploads/{subFolder}/{fileName}";
        }

        private void DeleteFile(string? fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) return;
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, fileUrl.TrimStart('/'));
            if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null) return NotFound();

            var model = new CreateQuestionViewModel
            {
                Id = question.Id, // Đảm bảo gán Id để dùng trong form
                QuestionText = question.QuestionText,
                LicenseCategoryId = question.LicenseCategoryId,
                Explanation = question.Explanation,
                IsCritical = question.IsCritical,
                QuestionType = question.QuestionType,
                Answers = question.Answers.Select(a => new AnswerViewModel
                {
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            ViewBag.Categories = _context.LicenseCategories.ToList();
            ViewBag.ImageUrl = question.ImageUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateQuestionViewModel model)
        {
            var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null) return NotFound();

            if (ModelState.IsValid)
            {
                // 1. Xử lý ảnh câu hỏi
                if (model.ImageFile != null) // Có upload ảnh mới
                {
                    DeleteFile(question.ImageUrl);
                    question.ImageUrl = await SaveFile(model.ImageFile, "questions");
                }
                else if (string.IsNullOrEmpty(Request.Form["ImageUrl"])) // Bấm nút xóa ảnh
                {
                    DeleteFile(question.ImageUrl);
                    question.ImageUrl = null;
                }

                // 2. Cập nhật thông tin cơ bản
                question.QuestionText = model.QuestionText;
                question.Explanation = model.Explanation;
                question.IsCritical = model.IsCritical;
                question.LicenseCategoryId = model.LicenseCategoryId;

                // 3. Xử lý đáp án (Xóa cũ thêm mới cho đơn giản hoặc Update từng cái)
                _context.Answers.RemoveRange(question.Answers);
                foreach (var a in model.Answers)
                {
                    string? finalAnsImg = a.imgUrl; // Giữ ảnh cũ mặc định

                    if (a.ImageFile != null) // Có upload ảnh mới cho đáp án
                    {
                        DeleteFile(a.imgUrl);
                        finalAnsImg = await SaveFile(a.ImageFile, "answers");
                    }
                    else if (string.IsNullOrEmpty(a.imgUrl)) // Admin bấm nút xóa ảnh đáp án
                    {
                        DeleteFile(a.imgUrl);
                        finalAnsImg = null;
                    }

                    question.Answers.Add(new Answer
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect,
                        ImageUrl = finalAnsImg
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // 1. Lấy thông tin câu hỏi và toàn bộ đáp án liên quan
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            try
            {
                // 2. Xóa ảnh của câu hỏi (nếu có)
                if (!string.IsNullOrEmpty(question.ImageUrl))
                {
                    DeleteFile(question.ImageUrl);
                }

                // 3. Duyệt danh sách đáp án để xóa ảnh của từng đáp án
                if (question.Answers != null && question.Answers.Any())
                {
                    foreach (var answer in question.Answers)
                    {
                        if (!string.IsNullOrEmpty(answer.ImageUrl))
                        {
                            DeleteFile(answer.ImageUrl);
                        }
                    }
                    // 4. Xóa các đáp án trong DB (Nếu bạn chưa cấu hình Cascade Delete trong DbContext)
                    _context.Answers.RemoveRange(question.Answers);
                }

                // 5. Xóa câu hỏi trong DB
                _context.Questions.Remove(question);

                await _context.SaveChangesAsync();

                // Thêm thông báo thành công (TempData) nếu cần
                TempData["Success"] = "Đã xóa câu hỏi và toàn bộ hình ảnh liên quan thành công!";
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                TempData["Error"] = "Có lỗi xảy ra khi xóa câu hỏi: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}