using ShopMvc.Data.Interfaces;
using ShopMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetByIdAsync(int? categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);            
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var category = await _context.Categories.FindAsync(name);

            return category;
        }

        public async Task Edit(Category category)
        {
            var entity = await _context.Categories.FindAsync(category.Id);
            if (entity == null)
                await _context.Categories.AddAsync(category);
            else
            {
                entity.Name = category.Name;
                //_context.Categories.FirstOrDefault(x => x.Id == category.Id).Name = category.Name;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories.AsEnumerable();

            return categories;
        }

        public IEnumerable<Liquid> GetLiquidsFromCategory(int? categoryId)
        {
            var liquids = _context.Liquids.Where(x => x.CategoryId == categoryId);

            return liquids;
        }
    }
}
