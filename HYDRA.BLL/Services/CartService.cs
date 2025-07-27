using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYDRA.DAL.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Add this using statement
using System.Collections.Generic; // Add this using statement


namespace HYDRA.BLL.Services
{
    public class CartService
    {
        private readonly HydraContext _context;

        public CartService()
        {
            _context = new HydraContext();
        }

        /// <summary>
        /// Finds the user's active shopping cart, or creates a new one if it doesn't exist.
        /// </summary>
        private ShoppingCart GetOrCreateCartForUser(int userId)
        {
            var cart = _context.ShoppingCarts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(cart);
                _context.SaveChanges();
            }
            return cart;
        }

        /// <summary>
        /// Adds a game to a user's shopping cart.
        /// </summary>
        public void AddGameToCart(int userId, int gameId)
        {
            var cart = GetOrCreateCartForUser(userId);

            // Business Rule: Don't add the same game twice.
            var itemExists = _context.CartItems.Any(ci => ci.CartId == cart.CartId && ci.GameId == gameId);
            if (itemExists)
            {
                // Optionally, you could show a message here. For now, we just do nothing.
                return;
            }

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                GameId = gameId
            };

            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves all items from a user's shopping cart, including game details.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of CartItem objects, or an empty list if the cart is empty or doesn't exist.</returns>
        public List<CartItem> GetCartItems(int userId)
        {
            // Find the user's cart first
            var cart = _context.ShoppingCarts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return new List<CartItem>(); // Return an empty list if no cart exists
            }

            // Find all cart items for that cart and include the related Game data for each item
            return _context.CartItems
                           .Include(ci => ci.Game) // This is the crucial part that loads game details
                           .Where(ci => ci.CartId == cart.CartId)
                           .ToList();
        }

        /// <summary>
        /// Removes a specific game from the user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the cart.</param>
        /// <param name="gameId">The ID of the game to remove.</param>
        public void RemoveGameFromCart(int userId, int gameId)
        {
            // Find the user's cart
            var cart = _context.ShoppingCarts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return;

            // Find the specific item in the cart
            var cartItemToRemove = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.GameId == gameId);

            if (cartItemToRemove != null)
            {
                _context.CartItems.Remove(cartItemToRemove);
                _context.SaveChanges();
            }
        }
    }
}