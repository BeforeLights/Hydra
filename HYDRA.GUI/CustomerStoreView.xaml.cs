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
using HYDRA.BLL.Services;
using HYDRA.DAL.Models;

namespace HYDRA.GUI
{
    public partial class CustomerStoreView : UserControl
    {
        private readonly GameService _gameService;
        private readonly MainWindow _mainWindow;
        private readonly User _currentUser;

        public CustomerStoreView(MainWindow mainWindow, User currentUser)
        {
            InitializeComponent();
            _gameService = new GameService();
            _mainWindow = mainWindow;
            _currentUser = currentUser;
            
            // Load all games initially
            LoadGamesForSale(null);
        }

        private void LoadGamesForSale(string searchTerm)
        {
            var games = _gameService.GetGamesForSale(searchTerm);
            GamesListBox.ItemsSource = games;
        }

        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGame = GamesListBox.SelectedItem as Game;
            if (selectedGame != null)
            {
                // Call the MainWindow to navigate to the detail view
                _mainWindow.NavigateTo(new GameDetailView(selectedGame.GameId, _currentUser));

                // Deselect the item to allow clicking it again
                GamesListBox.SelectedItem = null;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the text from the search box and call LoadGamesForSale with it
            LoadGamesForSale(SearchTextBox.Text);
        }
    }
}