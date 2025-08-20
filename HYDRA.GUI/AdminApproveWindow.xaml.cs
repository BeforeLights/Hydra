using Hydra.API.Services;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hydra.Gui
{
    public partial class AdminApproveWindow : Window
    {
        private readonly ApiClient _api = ApiClient.Default;
        private readonly int _adminUserId;
        private List<PendingItem> _all = new();
        private PendingItem? _selected;

        public AdminApproveWindow(int adminUserId)
        {
            InitializeComponent();
            _adminUserId = adminUserId;
            Loaded += async (_, __) => await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                _all = await _api.GetPendingAsync(200);
                grid.ItemsSource = _all;
                txtTotal.Text = $"({_all.Count})";
                if (_all.Count > 0)
                    grid.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "API", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e) => await LoadAsync();

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            var q = txtFilter.Text?.Trim();
            if (string.IsNullOrEmpty(q))
            {
                grid.ItemsSource = _all;
                txtTotal.Text = $"({_all.Count})";
                return;
            }
            var filtered = _all.Where(x => x.Title.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            grid.ItemsSource = filtered;
            txtTotal.Text = $"({filtered.Count})";
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtFilter.Text = "";
            grid.ItemsSource = _all;
            txtTotal.Text = $"({_all.Count})";
        }

        private async void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = grid.SelectedItem as PendingItem;
            await BindDetailAsync(_selected);
        }

        private async Task BindDetailAsync(PendingItem? item)
        {
            imgCover.Source = null;
            txtTitle.Text = txtDesc.Text = txtPlatforms.Text = txtGenres.Text =
                txtRelease.Text = txtPublishers.Text = "";
            txtVotes.Text = "0";

            if (item == null) return;

            txtTitle.Text = item.Title;
            txtVotes.Text = item.Votes.ToString();   // <-- HIỂN THỊ LƯỢT VOTE

            if (item.RawgId.HasValue)
            {
                try
                {
                    var d = await _api.GetRawgDetailAsync(item.RawgId.Value);
                    if (!string.IsNullOrWhiteSpace(d.BackgroundImage))
                    {
                        try { imgCover.Source = new BitmapImage(new Uri(d.BackgroundImage)); }
                        catch { /* ignore */ }
                    }
                    txtDesc.Text = d.Description ?? "";
                    txtPlatforms.Text = d.Platforms is { Length: > 0 } ? string.Join(", ", d.Platforms) : "—";
                    txtGenres.Text = d.Genres is { Length: > 0 } ? string.Join(", ", d.Genres) : "—";
                    txtRelease.Text = d.ReleaseDate.HasValue
                        ? d.ReleaseDate.Value.ToString("yyyy-MM-dd")
                        : (string.IsNullOrWhiteSpace(item.RawgReleased) ? "—" : item.RawgReleased);
                    txtPublishers.Text = d.Publishers is { Length: > 0 }
                        ? string.Join(", ", d.Publishers)
                        : (item.RawgPublishers ?? "—");
                }
                catch
                {
                    txtRelease.Text = string.IsNullOrWhiteSpace(item.RawgReleased) ? "—" : item.RawgReleased;
                    txtPublishers.Text = item.RawgPublishers ?? "—";
                }
            }
            else
            {
                txtRelease.Text = string.IsNullOrWhiteSpace(item.RawgReleased) ? "—" : item.RawgReleased;
                txtPublishers.Text = item.RawgPublishers ?? "—";
            }
        }

        private async void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null) return;

            if (!decimal.TryParse(txtPrice.Text, out var price) || price < 0)
            {
                MessageBox.Show("Default price không hợp lệ.", "Validate",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _api.ApproveAsync(_selected.Id, _adminUserId, txtNote.Text?.Trim(), price);
                MessageBox.Show("Approved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "API", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null) return;

            try
            {
                await _api.RejectAsync(_selected.Id, _adminUserId, txtNote.Text?.Trim());
                MessageBox.Show("Rejected!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "API", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
