using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYDRA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HYDRA.BLL.Services
{
    public class OrderService
    {
        private readonly HydraContext _context;

        public OrderService()
        {
            _context = new HydraContext();
        }

        /// <summary>
        /// Processes the entire checkout for a user. Creates an order, adds games
        /// to the library, and clears the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user checking out.</param>
        /// <returns>True if checkout was successful, false otherwise.</returns>
        public bool ProcessCheckout(int userId)
        {
            // 1. Find the user's cart and items
            var cart = _context.ShoppingCarts.Include(c => c.CartItems)
                                              .ThenInclude(ci => ci.Game)
                                              .FirstOrDefault(c => c.UserId == userId);

            // Business Rule: Can't check out with an empty cart
            if (cart == null || !cart.CartItems.Any())
            {
                return false;
            }

            // 2. Create the main Order record
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(item => item.Game.Price)
            };
            _context.Orders.Add(order);

            // 3. Create OrderItems and add games to the user's library
            foreach (var cartItem in cart.CartItems)
            {
                // Create an item for the order history
                var orderItem = new OrderItem
                {
                    Order = order, // Link to the new order
                    GameId = cartItem.GameId,
                    PriceAtTimeOfPurchase = cartItem.Game.Price
                };
                _context.OrderItems.Add(orderItem);

                // Create an entry for the user's personal library
                var libraryEntry = new LibraryEntry
                {
                    UserId = userId,
                    GameId = cartItem.GameId,
                    StatusId = 1, // Default to "In Library" or "Backlog"
                    DateAdded = DateTime.UtcNow
                };
                _context.LibraryEntries.Add(libraryEntry);
            }

            // 4. Clear the shopping cart
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.ShoppingCarts.Remove(cart);

            // 5. Save all changes to the database in a single transaction
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Retrieves a list of all past orders for a specific user.
        /// </summary>
        public List<Order> GetOrdersForUser(int userId)
        {
            return _context.Orders
                           .AsNoTracking()
                           .Where(o => o.UserId == userId)
                           .OrderByDescending(o => o.OrderDate) // Show newest orders first
                           .ToList();
        }

        /// <summary>`
        /// Retrieves all items (games) purchased in a specific order.
        /// </summary>
        /// <param name="orderId">The ID of the order to get items for.</param>
        /// <returns>A list of OrderItem objects with Game details included.</returns>
        public List<OrderItem> GetOrderItems(int orderId)
        {
            return _context.OrderItems
                           .AsNoTracking()
                           .Include(oi => oi.Game)
                           .ThenInclude(g => g.Publisher)
                           .Where(oi => oi.OrderId == orderId)
                           .ToList();
        }
    }
}