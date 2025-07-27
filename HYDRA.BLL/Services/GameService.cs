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
    public class GameService
    {
        private readonly HydraContext _context;

        public GameService()
        {
            _context = new HydraContext();
        }

        /// <summary>
        /// Retrieves a list of all games from the database.
        /// </summary>
        /// <returns>A list of Game objects.</returns>
        public List<Game> GetAllGames()
        {
            // Access the context's Games table and convert it to a list.
            // We use .AsNoTracking() for read-only operations to improve performance.
            return _context.Games.AsNoTracking().ToList();
        }

        /// <summary>
        /// Adds a new game to the database.
        /// </summary>
        /// <param name="game">The new game object to add.</param>
        public void AddGame(Game game)
        {
            // Basic validation rule
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            _context.Games.Add(game);
            _context.SaveChanges(); // This commits the new game to the database
        }

        /// <summary>
        /// Updates an existing game in the database.
        /// </summary>
        /// <param name="gameToUpdate">The game object with updated information.</param>
        public void UpdateGame(Game gameToUpdate)
        {
            _context.Games.Update(gameToUpdate);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a game from the database by its ID.
        /// </summary>
        /// <param name="gameId">The ID of the game to delete.</param>
        public void DeleteGame(int gameId)
        {
            // Find the game in the database first
            var gameToDelete = _context.Games.Find(gameId);
            if (gameToDelete != null)
            {
                _context.Games.Remove(gameToDelete);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves a list of games for sale, optionally filtered by a search term.
        /// </summary>
        /// <param name="searchTerm">The text to search for in game titles. If null or empty, all games for sale are returned.</param>
        /// <returns>A list of matching Game objects.</returns>
        public List<Game> GetGamesForSale(string searchTerm = null)
        {
            // Start with a query for all games for sale
            var query = _context.Games
                                .AsNoTracking()
                                .Where(g => g.IsForSale == true);

            // If a search term is provided, add another filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                // This filters the query to only include games where the title contains the search term
                query = query.Where(g => g.Title.Contains(searchTerm));
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets a single game by its ID, including its Publisher information.
        /// </summary>
        /// <param name="gameId">The ID of the game to retrieve.</param>
        /// <returns>A single Game object, or null if not found.</returns>
        public Game GetGameById(int gameId)
        {
            return _context.Games
                           .AsNoTracking()
                           .Include(g => g.Publisher) // Eagerly load the related Publisher
                           .Include(g => g.GameImages)
                           .FirstOrDefault(g => g.GameId == gameId);
        }


    }
}
