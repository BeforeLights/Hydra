using System;
using System.Collections.Generic;
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
using HYDRA.BLL.Services;
using HYDRA.DAL.Models;

namespace HYDRA.GUI
{
    public partial class MyLibraryView : UserControl
    {
        private readonly LibraryService _libraryService;
        private readonly MainWindow _mainWindow;
        private readonly User _currentUser;

        public MyLibraryView(int userId, MainWindow mainWindow, User currentUser)
        {
            InitializeComponent();
            _libraryService = new LibraryService();
            _mainWindow = mainWindow;
            _currentUser = currentUser;
            
            LoadLibraryGames(userId);
        }

        // Keep the old constructor for backward compatibility
        public MyLibraryView(int userId)
        {
            InitializeComponent();
            _libraryService = new LibraryService();
            LoadLibraryGames(userId);
        }

        private void LoadLibraryGames(int userId)
        {
            var games = _libraryService.GetLibraryGames(userId);
            GamesListBox.ItemsSource = games;
        }

        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGame = GamesListBox.SelectedItem as Game;
            if (selectedGame != null && _mainWindow != null && _currentUser != null)
            {
                // Navigate to the game detail view
                _mainWindow.NavigateTo(new GameDetailView(selectedGame.GameId, _currentUser));

                // Deselect the item to allow clicking it again
                GamesListBox.SelectedItem = null;
            }
        }
    }
}