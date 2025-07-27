using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYDRA.DAL.Models; // We need access to the User, Role, etc. models from the DAL
using Microsoft.EntityFrameworkCore; // Required for .Include()
using System.Collections.Generic;
using System.Linq;
using BCrypt.Net;

namespace HYDRA.BLL.Services
{
    public class UserService
    {
        private readonly HydraContext _context;

        public UserService()
        {
            // This creates an instance of our database context.
            // In a larger application, this would be handled by Dependency Injection,
            // but for simplicity, we'll create it directly here.
            _context = new HydraContext();
        }

        /// <summary>
        /// Retrieves a list of all users and their roles.
        /// </summary>
        public List<User> GetAllUsers()
        {
            return _context.Users
                           .Include(u => u.Role) // Eagerly load the Role navigation property
                           .AsNoTracking()
                           .ToList();
        }

        /// <summary>
        /// Updates a user's role.
        /// </summary>
        public void UpdateUserRole(int userId, int roleId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.RoleId = roleId;
                _context.SaveChanges();
            }
        }


        /// <summary>
        /// Deletes a user from the database.
        /// WARNING: In a real app, this is dangerous. Consider "soft deleting" instead.
        /// </summary>
        /// <summary>
        /// Deletes a user and all their associated data (orders, library, cart) from the database.
        /// </summary>
        public void DeleteUser(int userId)
        {
            // Find the user to delete
            var userToDelete = _context.Users.Find(userId);
            if (userToDelete == null) return;

            // --- 1. Delete dependent records first ---

            // Find and delete all orders and their items
            var orders = _context.Orders.Where(o => o.UserId == userId).ToList();
            foreach (var order in orders)
            {
                var orderItems = _context.OrderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                _context.OrderItems.RemoveRange(orderItems);
            }
            _context.Orders.RemoveRange(orders);

            // Find and delete all library entries
            var libraryEntries = _context.LibraryEntries.Where(le => le.UserId == userId).ToList();
            _context.LibraryEntries.RemoveRange(libraryEntries);

            // Find and delete the shopping cart and its items
            var shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.UserId == userId);
            if (shoppingCart != null)
            {
                var cartItems = _context.CartItems.Where(ci => ci.CartId == shoppingCart.CartId).ToList();
                _context.CartItems.RemoveRange(cartItems);
                _context.ShoppingCarts.Remove(shoppingCart);
            }

            // --- 2. Now it's safe to delete the user ---
            _context.Users.Remove(userToDelete);

            // --- 3. Save all changes in a single transaction ---
            _context.SaveChanges();
        }

        /// <summary>
        /// Handles the business logic for user login.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's plain-text password.</param>
        /// <returns>The full User object if login is successful; otherwise, null.</returns>
        public User Login(string username, string password)
        {
            // --- Business Rule 1: Validate input ---
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null; // Or throw a specific exception
            }

            // --- Data Access: Get user from database ---
            // We use the _context to access the Users table.
            // .Include(u => u.Role) tells EF Core to also load the related Role data.
            var user = _context.Users
                               .Include(u => u.Role)
                               .FirstOrDefault(u => u.Username == username);

            // --- Business Rule 2: Check if user exists ---
            if (user == null)
            {
                return null; // User not found
            }

            // --- Business Rule 3: Verify the password ---
            // WARNING: This is a simple string comparison for demonstration only.
            // In a real-world application, you MUST use a secure hashing library
            // like BCrypt.Net to hash passwords and verify them safely.
            // OLD: if (user.HashedPassword == password)
            // NEW: Use BCrypt.Verify to securely compare the plain-text password with the stored hash.
            if (BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
            {
                // Login successful!
                return user;
            }

            // If we reach here, the password was incorrect.
            return null;
        }

        /// <summary>
        /// Registers a new user in the system with a specified role.
        /// </summary>
        /// <returns>Returns an error message if registration fails, or null if it succeeds.</returns>
        public string RegisterUser(string username, string email, string password, int roleId)
        {
            // --- Business Rule 1: Check for existing user ---
            if (_context.Users.Any(u => u.Username == username))
            {
                return "Username is already taken.";
            }
            if (_context.Users.Any(u => u.Email == email))
            {
                return "An account with this email already exists.";
            }

            // --- Business Rule 2: Create the new user object ---
            var newUser = new User
            {
                Username = username,
                Email = email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12),
                RoleId = roleId, // Use the roleId passed from the registration form
                DateCreated = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return null; // Return null to indicate success
        }

        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
