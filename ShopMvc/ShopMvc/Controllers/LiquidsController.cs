using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopMvc.Data;
using ShopMvc.Data.Interfaces;
using ShopMvc.Models;

namespace ShopMvc.Controllers
{
    public class LiquidsController : Controller
    {
        private ILiquidRepository _repository;

        public LiquidsController(ILiquidRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liquid = _repository.GetById(id);
            if (liquid == null)
            {
                return NotFound();
            }

            return View(liquid);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int? id)
        {
            ViewData["CategoryId"] = id;
            ViewData["Id"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Liquid liquid)
        {
            try
            {
                await _repository.Add(liquid);
            }
            catch
            {
                HttpContext.Response.Cookies.Append("Error: can't save in db.", DateTime.Now.ToString("dd/MM/yyyy hh-mm-ss"));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Liquid liquid)
        {
            if (id != liquid.Id)
            {
                return NotFound();
            }
            if (liquid == null)
            {
                StatusCode(500);               
            }
            await _repository.Edit(liquid);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liquid = _repository.GetById(id);

            if (liquid == null)
            {
                return NotFound();
            }

            return View(liquid);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var liquid = _repository.GetById(id);
            await _repository.Delete(liquid);
            return RedirectToAction(nameof(Index));
        }

        //private bool CategoryExists(int id)
        //{
        //    return _repository.Categories.Any(e => e.Id == id);
        //}
    }
}
