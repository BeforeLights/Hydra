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

namespace HYDRA.GUI
{
    public partial class MyLibraryView : UserControl
    {
        private readonly LibraryService _libraryService;

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
    }
}