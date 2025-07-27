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
    }
}