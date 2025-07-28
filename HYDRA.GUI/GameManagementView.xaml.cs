using HYDRA.BLL.Services;
using HYDRA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HYDRA.GUI
{
    public partial class GameManagementView : UserControl
    {
        private readonly GameService _gameService;

        public GameManagementView()
        {
            InitializeComponent();
            _gameService = new GameService();
            LoadGames();
        }

        private void LoadGames()
        {
            // Get the list of games from the BLL
            var games = _gameService.GetAllGames();

            // Sort by title to make duplicates easier to spot
            games = games.OrderBy(g => g.Title).ThenBy(g => g.GameId).ToList();

            // Set the DataGrid's data source to our list of games
            GamesDataGrid.ItemsSource = games;

            // Check for duplicates and show a warning
            var duplicates = games.GroupBy(g => g.Title.ToLower())
                                 .Where(group => group.Count() > 1)
                                 .ToList();

            if (duplicates.Any())
            {
                var duplicateCount = duplicates.Sum(g => g.Count());
                var uniqueTitles = duplicates.Count();
                MessageBox.Show($"Warning: Found {duplicateCount} games with {uniqueTitles} duplicate title(s). " +
                               "Consider cleaning up duplicate entries.", 
                               "Duplicate Games Detected", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
            }
        }

        private void AddNewGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of our editor window
            GameEditorWindow editorWindow = new GameEditorWindow();

            // Show it as a dialog, which pauses this window until the editor is closed
            bool? result = editorWindow.ShowDialog();

            // Check if the user clicked "Save" in the editor window
            if (result == true)
            {
                // If they saved, reload the list of games to show the new one
                LoadGames();
            }
        }

        // Add this method for the Edit button
        private void EditGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected game from the grid
            var selectedGame = GamesDataGrid.SelectedItem as Game;
            if (selectedGame == null)
            {
                MessageBox.Show("Please select a game to edit.", "Selection Required");
                return;
            }

            // Open the editor window, passing the selected game to the new constructor
            GameEditorWindow editorWindow = new GameEditorWindow(selectedGame);
            if (editorWindow.ShowDialog() == true)
            {
                LoadGames(); // Refresh the list if changes were saved
            }
        }

        // Add this method for the Delete button
        private void DeleteGameButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGame = GamesDataGrid.SelectedItem as Game;
            if (selectedGame == null)
            {
                MessageBox.Show("Please select a game to delete.", "Selection Required");
                return;
            }

            // Show a confirmation dialog
            var result = MessageBox.Show($"Are you sure you want to delete '{selectedGame.Title}'?",
                                         "Confirm Deletion",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _gameService.DeleteGame(selectedGame.GameId);
                LoadGames(); // Refresh the list
            }
        }

        private void GamesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    // Remove the BoolToColorConverter and BoolToTextConverter classes from here - they're now in SharedConverters.cs
}