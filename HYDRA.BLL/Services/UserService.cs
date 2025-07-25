using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYDRA.DAL.Models; // We need access to the User, Role, etc. models from the DAL
using Microsoft.EntityFrameworkCore; // Required for .Include()

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
            if (user.HashedPassword == password)
            {
                // Login successful! Return the user object with all its data.
                return user;
            }

            // If we reach here, the password was incorrect.
            return null;
        }
    }
}
