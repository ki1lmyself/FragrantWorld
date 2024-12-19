using FragrantWorldApp.Data;
using FragrantWorldApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FragrantWorldApp.Services
{
    public class ProductService
    {
        private readonly FragrantWorldContext _context;

        public ProductService(FragrantWorldContext context)
        {
            _context = context;
        }

        // Метод для получения всех продуктов
        public async Task<List<ExamProduct>> GetProductsAsync()
        {
            using var context = new FragrantWorldContext();
            return await _context.ExamProducts.ToListAsync();
        }

        // Метод для получения всех продуктов
        public async Task<List<string>> GetManufacturersAsync()
        {
            return await _context.ExamProducts
                .Select(p => p.ProductManufacturer)
                .ToListAsync();
        }

        // Метод для сортировки товаров по стоимости
        public async Task<List<ExamProduct>> SortProductsAsync(bool ascending)
        {
            return ascending
                ? await _context.ExamProducts.OrderBy(p => p.ProductCost).ToListAsync()
                : await _context.ExamProducts.OrderByDescending(p => p.ProductCost).ToListAsync();
        }

        // Метод для фильтрации по производителю
        public async Task<List<ExamProduct>> FilterProductsByManufacturerAsync(string manufacturer)
        {
            if (manufacturer == "Все производители")
                return await _context.ExamProducts.ToListAsync();

            return await _context.ExamProducts.Where(p => p.ProductManufacturer == manufacturer).ToListAsync();
        }

        // Метод для фильтрации по цене
        public async Task<List<ExamProduct>> FilterProductsByPriceAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.ExamProducts
                .Where(p => p.ProductCost >= minPrice && p.ProductCost <= maxPrice)
                .ToListAsync();
        }

        // Метод для поиска по названию
        public async Task<List<ExamProduct>> SearchProductsByNameAsync(string manufacturer)
        {
            return await _context.ExamProducts
                .Where(p => p.ProductManufacturer == manufacturer || manufacturer == "Все производители")
                .ToListAsync();
        }

        // Объединение функиональности
        public async Task<List<ExamProduct>> GetFilteredSortedSearchedProductsAsync
            (string manufacturer, decimal? minPrice, decimal? maxPrice, string searchString, bool ascending)
        {
            IQueryable<ExamProduct> query = _context.ExamProducts;

            if (!string.IsNullOrEmpty(manufacturer) && manufacturer != "Все производители")
                query = query.Where(p => p.ProductManufacturer == manufacturer);

            if (minPrice.HasValue)
                query = query.Where(p => p.ProductCost >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.ProductCost <= maxPrice.Value);

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(p => p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            return ascending
                ? await query.OrderBy(p => p.ProductCost).ToListAsync()
                : await query.OrderByDescending(p => p.ProductCost).ToListAsync();
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            return await _context.ExamProducts.CountAsync();
        }
    }
}
