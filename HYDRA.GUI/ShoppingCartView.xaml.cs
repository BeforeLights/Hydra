using HYDRA.BLL.Services;
using HYDRA.DAL.Models;
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

namespace HYDRA.GUI
{
    public partial class ShoppingCartView : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly User _currentUser;
        private readonly OrderService _orderService;
        private readonly CartService _cartService;

        public ShoppingCartView(MainWindow mainWindow, User currentUser)
        {
            InitializeComponent();
            _cartService = new CartService();
            _orderService = new OrderService();
            _mainWindow = mainWindow;
            _currentUser = currentUser;
            LoadCartItems(_currentUser.UserId);
        }

        private void LoadCartItems(int userId)
        {
            var cartItems = _cartService.GetCartItems(userId);
            CartDataGrid.ItemsSource = cartItems;

            // Calculate and display the total price
            decimal totalPrice = cartItems.Sum(item => item.Game.Price);
            TotalPriceTextBlock.Text = $"Total: {totalPrice:C}";
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = _orderService.ProcessCheckout(_currentUser.UserId);

            if (success)
            {
                MessageBox.Show("Checkout successful! Your new games have been added to your library.", "Success");
                _mainWindow.NavigateTo(new MyLibraryView(_currentUser.UserId));
            }
            else
            {
                MessageBox.Show("Your cart is empty.", "Checkout Failed");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            int gameIdToRemove = (int)button.CommandParameter;
            _cartService.RemoveGameFromCart(_currentUser.UserId, gameIdToRemove);
            LoadCartItems(_currentUser.UserId);
        }
    }
}