using ShopMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Data.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Liquid> GetLiquidsFromCategory(int? categoryId);
        Task Add(Category category);
        Task<Category> GetByIdAsync(int? categoryId);
        Task<Category> GetByNameAsync(string name);
        Task Edit(Category category);
        Task Delete(Category category);
        IEnumerable<Category> GetAll();
    }
}
