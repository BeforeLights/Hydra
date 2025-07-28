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
using System.Collections.Generic;

namespace HYDRA.GUI
{
    public partial class MainWindow : Window
    {
        private readonly User _currentUser;
        private readonly Stack<UserControl> _navigationHistory = new Stack<UserControl>();

        public MainWindow(User loggedInUser)
        {
            InitializeComponent();
            _currentUser = loggedInUser;
            SetupWelcomeText();
            SetupUIForUser();
        }

        private void SetupWelcomeText()
        {
            WelcomeText.Text = $"Welcome, {_currentUser.Username} ({_currentUser.Role.RoleName})";
        }

        public void NavigateTo(UserControl newView)
        {
            if (MainContentArea.Content is UserControl currentView)
            {
                _navigationHistory.Push(currentView);
            }

            MainContentArea.Content = newView;
            UpdateBackButtonVisibility();
        }

        private void GoBack()
        {
            if (_navigationHistory.Count > 0)
            {
                MainContentArea.Content = _navigationHistory.Pop();
                UpdateBackButtonVisibility();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void UpdateBackButtonVisibility()
        {
            BackButton.Visibility = _navigationHistory.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private Button CreateNavigationButton(string content, string icon = null)
        {
            var button = new Button
            {
                Content = string.IsNullOrEmpty(icon) ? content : $"{icon} {content}",
                Style = (Style)FindResource("ModernButton"),
                Margin = new Thickness(5, 0, 5, 0),
                Padding = new Thickness(20, 10, 20, 10),
                FontSize = 14
            };
            return button;
        }

        private void SetupUIForUser()
        {
            HeaderPanel.Children.Clear();
            _navigationHistory.Clear();

            switch (_currentUser.Role.RoleName)
            {
                case "Customer":
                    var storeButton = CreateNavigationButton("Store", "🛒");
                    var cartButton = CreateNavigationButton("Cart", "🛍️");
                    var libraryButton = CreateNavigationButton("Library", "📚");
                    var ordersButton = CreateNavigationButton("Orders", "📋");

                    storeButton.Click += (s, e) => NavigateTo(new CustomerStoreView(this, _currentUser));
                    cartButton.Click += (s, e) => NavigateTo(new ShoppingCartView(this, _currentUser));
                    libraryButton.Click += (s, e) => NavigateTo(new MyLibraryView(_currentUser.UserId, this, _currentUser));
                    ordersButton.Click += (s, e) => NavigateTo(new MyOrdersView(_currentUser.UserId));

                    HeaderPanel.Children.Add(storeButton);
                    HeaderPanel.Children.Add(cartButton);
                    HeaderPanel.Children.Add(libraryButton);
                    HeaderPanel.Children.Add(ordersButton);

                    NavigateTo(new CustomerStoreView(this, _currentUser));
                    break;

                case "Admin":
                    var userMgmtButton = CreateNavigationButton("User Management", "👥");
                    var gameMgmtButton = CreateNavigationButton("Game Management", "🎮");

                    userMgmtButton.Click += (s, e) => NavigateTo(new UserManagementView());
                    gameMgmtButton.Click += (s, e) => NavigateTo(new GameManagementView());

                    HeaderPanel.Children.Add(userMgmtButton);
                    HeaderPanel.Children.Add(gameMgmtButton);

                    NavigateTo(new UserManagementView());
                    break;

                case "Manager":
                    var managerGameMgmtButton = CreateNavigationButton("Game Management", "🎮");
                    managerGameMgmtButton.Click += (s, e) => NavigateTo(new GameManagementView());
                    HeaderPanel.Children.Add(managerGameMgmtButton);
                    
                    NavigateTo(new GameManagementView());
                    break;
            }
            UpdateBackButtonVisibility();
        }
    }
}