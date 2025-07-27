using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYDRA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HYDRA.BLL.Services
{
    public class LibraryService
    {
        private readonly HydraContext _context;

        public LibraryService()
        {
            _context = new HydraContext();
        }

        /// <summary>
        /// Retrieves all games owned by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of Game objects from the user's library.</returns>
        public List<Game> GetLibraryGames(int userId)
        {
            // Find all library entries for the user,
            // select the associated Game object for each entry,
            // and return it as a list.
            return _context.LibraryEntries
                           .Where(le => le.UserId == userId)
                           .Select(le => le.Game) // This is the key part that gets the Game details
                           .AsNoTracking()
                           .ToList();
        }

        /// <summary>
        /// Checks if a specific game is already in a user's library.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="gameId">The ID of the game to check.</param>
        /// <returns>True if the user owns the game, false otherwise.</returns>
        public bool IsGameInLibrary(int userId, int gameId)
        {
            return _context.LibraryEntries.Any(le => le.UserId == userId && le.GameId == gameId);
        }
    }
}