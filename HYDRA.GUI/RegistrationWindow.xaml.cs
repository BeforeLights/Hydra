﻿using System;
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
    public partial class RegistrationWindow : Window
    {
        private readonly UserService _userService;

        public RegistrationWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            // Load the roles into the dropdown when the window is created
            LoadRoles();
        }

        private void LoadRoles()
        {
            RoleComboBox.ItemsSource = _userService.GetAllRoles();
            // Set the default selection to the first item (Customer)
            RoleComboBox.SelectedIndex = 0;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // --- NEW: Input Validation Section ---

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stop the process
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !EmailTextBox.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stop the process
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stop the process
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Stop the process
            }

            if (RoleComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // --- End of Validation Section ---


            // Get the selected role ID from the ComboBox
            int selectedRoleId = (int)RoleComboBox.SelectedValue;

            // Call the BLL to register the user
            string result = _userService.RegisterUser(
                UsernameTextBox.Text,
                EmailTextBox.Text,
                PasswordBox.Password,
                selectedRoleId
            );

            if (result == null) // Success
            {
                MessageBox.Show("Registration successful! You can now log in.", "Success");
                this.Close();
            }
            else // Failure from BLL (e.g., username taken)
            {
                MessageBox.Show(result, "Registration Failed");
            }
        }

        private void BackToLoginHyperlink_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}