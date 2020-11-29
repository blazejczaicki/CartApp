using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CartApp.Data;
using CartApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CartApp.Controllers
{
    /// <summary>
    /// Controller class for manage products shop.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopCartDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(ShopCartDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Show products.
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductSet.ToListAsync());
        }

        /// <summary>
        /// Show details.
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductSet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Show create menu.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price, ImageFile")] Product product)
        {
            var imgFile = product.ImageFile;

            using (var ms = new MemoryStream())
            {
                imgFile.CopyTo(ms);
                product.Image = ms.ToArray();
            }


            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        /// <summary>
        /// Show edit menu of product.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductSet.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Edit product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        /// <summary>
        /// Show remove menu of product.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.ProductSet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Remove product.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.ProductSet.FindAsync(id);
            _context.ProductSet.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check product exist.
        /// </summary>
        private bool ProductExists(int id)
        {
            return _context.ProductSet.Any(e => e.Id == id);
        }
    }
}
