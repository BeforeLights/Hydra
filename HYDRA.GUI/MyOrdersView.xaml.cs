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
    public partial class MyOrdersView : UserControl
    {
        private readonly OrderService _orderService;
        
        public MyOrdersView(int userId)
        {
            InitializeComponent();
            _orderService = new OrderService();
            OrdersDataGrid.ItemsSource = _orderService.GetOrdersForUser(userId);
        }

        private void ViewItemsButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            var button = sender as Button;
            if (button == null) return;

            // Get the OrderId from the CommandParameter
            int orderId = (int)button.CommandParameter;

            // Get the order items for this order
            var orderItems = _orderService.GetOrderItems(orderId);

            if (orderItems.Any())
            {
                // Create a formatted message showing all purchased games
                var message = new StringBuilder();
                message.AppendLine($"Games purchased in Order #{orderId}:");
                message.AppendLine();

                foreach (var item in orderItems)
                {
                    message.AppendLine($"• {item.Game.Title}");
                    message.AppendLine($"  Publisher: {item.Game.Publisher?.Name ?? "Unknown"}");
                    message.AppendLine($"  Price: {item.PriceAtTimeOfPurchase:C}");
                    message.AppendLine();
                }

                // Show the purchased games in a message box
                MessageBox.Show(message.ToString(), 
                               $"Order #{orderId} - Purchased Games", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"No items found for Order #{orderId}.", 
                               "No Items", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Information);
            }
        }
    }
}