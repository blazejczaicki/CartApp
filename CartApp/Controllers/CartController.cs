using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartApp.Data;
using CartApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopCartDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public CartController(ShopCartDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {
            var cartProductSet= await  _context.CartProductSet.ToListAsync();
            var productSet= await  _context.ProductSet.ToListAsync();
            var currentUser = await GetCurrentUserAsync();

            var userProducts = cartProductSet.FindAll(x => x.UserId == currentUser.Id);

            List<CartProductViewModel> cartProducts = new List<CartProductViewModel>();
            foreach (var item in userProducts)
            {
                Product product = productSet.Find(x => x.Id == item.Id);
                CartProductViewModel cartProduct = new CartProductViewModel {
                    Id = product.Id,
                    image = product.Image,
                    productName = product.Name,
                    price = product.Price,
                    count = item.Count,
                    subtotal = item.Count * product.Price
                };
                cartProducts.Add(cartProduct);
            }

            return View(cartProducts);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int? id)
        {
            var products = await _context.ProductSet.ToListAsync();
            if (id == null || !products.Exists(x => x.Id==(int)id))
            {
                return NotFound();
            }

            CartProductViewModel product = new CartProductViewModel { Id = (int)id };

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartProductViewModel model)//int? productId, int count
        {                                                                       //stackowanie
            var user = await GetCurrentUserAsync();
            var product = new CartProduct { ProductId = model.Id, UserId = user.Id, Count = model.count };

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult ReleaseOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReleaseOrder([Bind("FirstName,LastName,Country, StreetAdress, Email")] Order order)
        {
            if (ModelState.IsValid)
            {
                var cartProductSet = await _context.CartProductSet.ToListAsync();
                var productSet = await _context.ProductSet.ToListAsync();
                var currentUser = await GetCurrentUserAsync();

                order.Products = cartProductSet.FindAll(x => x.UserId == currentUser.Id);
                //usuwanie id przedmiotu i usera

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
    }
}
