using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMvc.Data;
using ShopMvc.Data.Interfaces;
using ShopMvc.Data.Repositories;
using ShopMvc.Filters;
using ShopMvc.Models;

namespace ShopMvc.Controllers
{
    [Route("CategoriesPanel/[controller]/")]
    public class CategoriesController : Controller
    {
        private ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [CheckActionFilter]
        [Route("All")]
        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Info")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _repository.GetByIdAsync(id).Result;
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[action]")]
        public async Task<IActionResult> Create(Category category)
        {
            await _repository.Add(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[action]")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _repository.GetByIdAsync(id).Result;
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id || category == null)
            {
                return NotFound();
            }

            await _repository.Edit(category);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[action]")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _repository.GetByIdAsync(id).Result;

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = _repository.GetByIdAsync(id);
            await _repository.Delete(category.Result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        [Route("Liquids")]
        public IActionResult GetLiquidsFromCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liquids = _repository.GetLiquidsFromCategory(id);
            var category = _repository.GetByIdAsync(id).Result;
            ViewData["CategoryName"] = category.Name;
            ViewData["CategoryId"] = category.Id;
            return View(liquids);
        }

        //private bool CategoryExists(int id)
        //{
        //    return _repository.Categories.Any(e => e.Id == id);
        //}
    }
}
