using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HYDRA.DAL.Models;
using System.Collections.Generic; // Required for Stack<T>

namespace HYDRA.GUI
{
    public partial class MainWindow : Window
    {
        private readonly User _currentUser;
        // This stack will hold our navigation history.
        private readonly Stack<UserControl> _navigationHistory = new Stack<UserControl>();

        public MainWindow(User loggedInUser)
        {
            InitializeComponent();
            _currentUser = loggedInUser;
            SetupUIForUser();
        }

        // --- THE NEW NAVIGATION SYSTEM ---

        public void NavigateTo(UserControl newView)
        {
            // Before we navigate to a new view, we push the *current* one onto the history stack.
            if (MainContentArea.Content is UserControl currentView)
            {
                _navigationHistory.Push(currentView);
            }

            // Now, show the new view.
            MainContentArea.Content = newView;
            UpdateBackButtonVisibility();
        }

        private void GoBack()
        {
            // If there's anything in our history, pop it off and display it.
            if (_navigationHistory.Count > 0)
            {
                MainContentArea.Content = _navigationHistory.Pop();
                UpdateBackButtonVisibility();
            }
        }

        // Add this click handler method
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        // In the SetupUIForUser method, find and DELETE these two lines:
        // var backButton = new Button { Name = "BackButton", Content = "← Back", Margin = new Thickness(0, 0, 10, 0) };
        // backButton.Click += (s, e) => GoBack();
        // HeaderPanel.Children.Add(backButton);

        private void UpdateBackButtonVisibility()
        {
            // Only show the back button if there is something to go back to.
            BackButton.Visibility = _navigationHistory.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the login window
            LoginWindow loginWindow = new LoginWindow();

            // Show the new login window
            loginWindow.Show();

            // Close the current main window, effectively logging the user out
            this.Close();
        }

        // --- UI SETUP (MODIFIED TO USE THE NEW NAVIGATION) ---
        private void SetupUIForUser()
        {
            HeaderPanel.Children.Clear();
            _navigationHistory.Clear(); // Clear history for the new user session

            switch (_currentUser.Role.RoleName)
            {
                case "Customer":
                    var storeButton = new Button { Content = "Store", Margin = new Thickness(10, 0, 0, 0) };
                    var cartButton = new Button { Content = "View Cart", Margin = new Thickness(10, 0, 0, 0) };
                    var libraryButton = new Button { Content = "My Library", Margin = new Thickness(10, 0, 0, 0) };
                    var ordersButton = new Button { Content = "My Orders", Margin = new Thickness(10, 0, 0, 0) };

                    storeButton.Click += (s, e) => NavigateTo(new CustomerStoreView(this, _currentUser));
                    cartButton.Click += (s, e) => NavigateTo(new ShoppingCartView(this, _currentUser));
                    libraryButton.Click += (s, e) => NavigateTo(new MyLibraryView(_currentUser.UserId));
                    ordersButton.Click += (s, e) => NavigateTo(new MyOrdersView(_currentUser.UserId));

                    HeaderPanel.Children.Add(storeButton);
                    HeaderPanel.Children.Add(cartButton);
                    HeaderPanel.Children.Add(libraryButton);
                    HeaderPanel.Children.Add(ordersButton);

                    NavigateTo(new CustomerStoreView(this, _currentUser));
                    break;

                case "Admin":
                    var userMgmtButton = new Button { Content = "User Management", Margin = new Thickness(10, 0, 0, 0) };
                    var gameMgmtButton = new Button { Content = "Game Management", Margin = new Thickness(10, 0, 0, 0) };

                    userMgmtButton.Click += (s, e) => NavigateTo(new UserManagementView());
                    gameMgmtButton.Click += (s, e) => NavigateTo(new GameManagementView());

                    HeaderPanel.Children.Add(userMgmtButton);
                    HeaderPanel.Children.Add(gameMgmtButton);

                    NavigateTo(new UserManagementView());
                    break;

                case "Manager":
                    NavigateTo(new GameManagementView());
                    break;
            }
            UpdateBackButtonVisibility(); // Ensure back button is hidden on initial view
        }
    }
}