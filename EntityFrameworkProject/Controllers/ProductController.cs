using EntityFrameworkProject.Data;
using EntityFrameworkProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.Id)
                .Take(6)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ProductImages.FirstOrDefault().Image
                })
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> LoadMore(int skip)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.Id)
                .Skip(skip)
                .Take(6)
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ProductImages.FirstOrDefault().Image
                })
                .ToListAsync();

            return PartialView("_ProductPartial", products);
        }
    }
}
