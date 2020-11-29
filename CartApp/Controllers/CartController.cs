using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CartApp.Data;
using CartApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartApp.Controllers
{
    /// <summary>
    /// Controller class for manage cart.
    /// </summary>
    [Authorize]
    public class CartController : Controller
    {
        private readonly ShopCartDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public CartController(ShopCartDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get current user.
        /// </summary>
        private Task<IdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        /// <summary>
        /// Show user cart.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var cartProductSet= await  _context.CartProductSet.ToListAsync();
            var productSet= await  _context.ProductSet.ToListAsync();
            var currentUser = await GetCurrentUserAsync();

            var userProducts = cartProductSet.FindAll(x => x.UserId == currentUser.Id);

            userProducts.RemoveAll(x => x.IsOrdered);

            List<CartProductViewModel> cartProducts = new List<CartProductViewModel>();
            foreach (var item in userProducts)
            {
                Product product = productSet.Find(x => x.Id == item.ProductId);
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

        /// <summary>
        /// Show add to cart product menu.
        /// </summary>
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

        /// <summary>
        /// Add product to cart.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartProductViewModel model)
        {   
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var cartProductSet = await _context.CartProductSet.ToListAsync();
                var cartProduct = cartProductSet.Find(x => x.ProductId == model.Id && x.UserId==user.Id && x.IsOrdered==false);
                if (cartProduct != null)
                {
                    cartProduct.Count += model.count;
                    _context.Attach(cartProduct);
                    _context.Entry(cartProduct).Property(x=>x.Count).IsModified=true;
                }
                else
                {
                    var product = new CartProduct { ProductId = model.Id, UserId = user.Id, Count = model.count };
                    _context.Add(product);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        /// <summary>
        /// Show release order menu.
        /// </summary>
        [HttpGet]
        public IActionResult ReleaseOrder()
        {
            return View();
        }

        /// <summary>
        /// Release order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ReleaseOrder([Bind("FirstName,LastName,Country, StreetAdress, Email")] Order order)
        {
            if (ModelState.IsValid)
            {
                var cartProductSet = await _context.CartProductSet.ToListAsync();
                var productSet = await _context.ProductSet.ToListAsync();
                var currentUser = await GetCurrentUserAsync();

                order.Products = cartProductSet.FindAll(x => x.UserId == currentUser.Id);
                order.Products.ForEach(x => x.IsOrdered = true);
                _context.Add(order);
                await _context.SaveChangesAsync();
                return View("OrderSent");
            }
            return View(order);
        }

        /// <summary>
        /// Show list of orders.
        /// </summary>
        public async Task<IActionResult> ListOrders()
        {
            return View(await _context.OrdersSet
                .Include(x => x.Products)
                .ToListAsync());
        }

        /// <summary>
        /// Show order details.
        /// </summary>
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrdersSet
                .Include(x=> x.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// Remove cart item.
        /// </summary>
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var products = await _context.CartProductSet.ToListAsync();
            var product = products.Find(x => x.ProductId == id);
            _context.CartProductSet.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Remove cart order.
        /// </summary>
        public async Task<IActionResult> DeleteCartOrder(int id)
        {
            var orders = await _context.OrdersSet
                .Include(x => x.Products)
                .ToListAsync();
            var order=orders.Find(x=> x.Id==id);
            _context.CartProductSet.RemoveRange(order.Products);
            _context.OrdersSet.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListOrders));
        }
    }
}
