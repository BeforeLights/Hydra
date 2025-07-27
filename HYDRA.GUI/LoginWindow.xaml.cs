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
using System.Windows.Shapes;
using HYDRA.BLL.Services;

namespace HYDRA.GUI
{
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Get input from the UI
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // 2. Ask the BLL to perform the login
            var user = _userService.Login(username, password);

            // 3. Check the result and act accordingly
            if (user != null)
            {
                // --- SUCCESS ---
                // Create an instance of our main application window
                MainWindow mainWindow = new MainWindow(user);

                // Show the main window
                mainWindow.Show();

                // Close this login window
                this.Close();
            }
            else
            {
                // --- FAILURE ---
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }
    }
}