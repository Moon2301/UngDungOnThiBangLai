using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UngDungOnThiBangLai.Models;

namespace UngDungOnThiBangLai.Controllers.Web
{
    public class LicenseCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public LicenseCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH
        public async Task<IActionResult> Index()
        {
            var categories = await _context.LicenseCategories
                .Include(c => c.Questions) // Để đếm số câu hỏi mỗi hạng
                .ToListAsync();
            return View(categories);
        }

        // 2. THÊM MỚI (GET)
        public IActionResult Create() => View();

        // 3. THÊM MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LicenseCategory model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 4. SỬA (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.LicenseCategories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // 5. SỬA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LicenseCategory category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 6. XÓA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.LicenseCategories.FindAsync(id);
            if (category == null) return NotFound();

            _context.LicenseCategories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}