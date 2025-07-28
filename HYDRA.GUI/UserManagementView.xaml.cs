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
    public partial class UserManagementView : UserControl
    {
        private readonly UserService _userService;

        public UserManagementView()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadUsers();
            LoadRoles();
        }

        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = _userService.GetAllUsers();
        }

        private void LoadRoles()
        {
            RoleComboBox.ItemsSource = _userService.GetAllRoles();
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to delete.", "Selection Required");
                return;
            }
            if (selectedUser.RoleId == 3) // Simple check for Admin role
            {
                MessageBox.Show("The Admin account cannot be deleted.", "Action Denied");
                return;
            }
            var result = MessageBox.Show($"Are you sure you want to delete user '{selectedUser.Username}'?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _userService.DeleteUser(selectedUser.UserId);
                LoadUsers();
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersDataGrid.SelectedItem as User;
            if (selectedUser == null || RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a user and a new role.", "Selection Required");
                return;
            }

            int newRoleId = (int)RoleComboBox.SelectedValue;
            _userService.UpdateUserRole(selectedUser.UserId, newRoleId);
            MessageBox.Show("User role updated successfully.");
            LoadUsers(); // Refresh the grid to show the change
        }
    }
}