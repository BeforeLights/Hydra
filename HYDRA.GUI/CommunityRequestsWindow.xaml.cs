using Hydra.API.Services;
using HYDRA.DAL.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Hydra.Gui
{
    public partial class CommunityRequestsWindow : Window
    {
        private readonly User _currentUser;
        private readonly ApiClient _api = ApiClient.Default;

        private TopSuggestionItem? _cur;

        private ObservableCollection<TopSuggestionItem> _all = new();
        private ObservableCollection<TopSuggestionItem> _view = new();
        private bool _initialized;
        public CommunityRequestsWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;

            Loaded += async (_, __) => await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                var list = await _api.GetTopSuggestionsAsync(_currentUser.UserId, 200);

                if (!_initialized)
                {
                    // Lần đầu: bind & cấu hình sort/filter
                    _view = new ObservableCollection<TopSuggestionItem>(list);
                    lv.ItemsSource = _view;

                    var cv = CollectionViewSource.GetDefaultView(lv.ItemsSource);
                    cv.SortDescriptions.Clear();
                    cv.SortDescriptions.Add(new SortDescription(nameof(TopSuggestionItem.votes), ListSortDirection.Descending));
                    cv.SortDescriptions.Add(new SortDescription(nameof(TopSuggestionItem.title), ListSortDirection.Ascending));

                    ApplyFilter();
                    if (lv.Items.Count > 0) lv.SelectedIndex = 0;

                    _initialized = true;
                }
                else
                {
                    // Các lần sau: cập nhật tại chỗ (KHÔNG thay ItemsSource)
                    Sync(_view, list);
                    CollectionViewSource.GetDefaultView(lv.ItemsSource)?.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "API");
            }
        }
        private static void Sync(ObservableCollection<TopSuggestionItem> target, IEnumerable<TopSuggestionItem> src)
        {
            target.Clear();
            foreach (var x in src) target.Add(x);
        }

        private void ApplyFilter()
        {
            string q = txtSearch.Text?.Trim() ?? "";
            var cv = CollectionViewSource.GetDefaultView(lv.ItemsSource);
            cv.Filter = o => o is TopSuggestionItem x &&
                             (string.IsNullOrWhiteSpace(q) ||
                              x.title.Contains(q, StringComparison.OrdinalIgnoreCase));
            cv.Refresh();
        }


        private void Refresh_Click(object sender, RoutedEventArgs e) => _ = LoadAsync();

        private void Search_Click(object sender, RoutedEventArgs e) => ApplyFilter();

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lv.SelectedItem is TopSuggestionItem it)
            {
                _cur = it;
                _ = LoadDetailAsync(it); // nạp detail
            }
        }
        private ListCollectionView View =>
    (ListCollectionView)CollectionViewSource.GetDefaultView(lv.ItemsSource);

        private bool _busy;
        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_busy) return;
            _cur = lv.SelectedItem as TopSuggestionItem;
            RenderRightPanel(_cur);
            UpdateButtons(_cur);
            if (_cur != null) _ = LoadDetailAsync(_cur);
        }

        private async Task LoadDetailAsync(TopSuggestionItem item)
        {
            try
            {
                RawgDetail? d = null;

                if (item.rawgId.HasValue)
                    d = await _api.GetRawgDetailAsync(item.rawgId.Value);
                else
                    d = await _api.GetRawgDetailByNameAsync(item.title);

                if (d != null)
                {
                    if (!string.IsNullOrWhiteSpace(d.BackgroundImage))
                        SetImage(imgCover, d.BackgroundImage);

                    txtDesc.Text = d.Description ?? txtDesc.Text;

                    txtPlatforms.Text = (d.Platforms is { Length: > 0 })
                        ? string.Join(", ", d.Platforms)
                        : (string.IsNullOrWhiteSpace(txtPlatforms.Text) ? "—" : txtPlatforms.Text);

                    txtGenres.Text = (d.Genres is { Length: > 0 })
                        ? string.Join(", ", d.Genres)
                        : (string.IsNullOrWhiteSpace(txtGenres.Text) ? "—" : txtGenres.Text);

                    txtRelease.Text = d.ReleaseDate.HasValue
                        ? d.ReleaseDate.Value.ToString("yyyy-MM-dd")
                        : (string.IsNullOrWhiteSpace(txtRelease.Text) ? "—" : txtRelease.Text);

                    txtPublishers.Text = (d.Publishers is { Length: > 0 })
                        ? string.Join(", ", d.Publishers)
                        : (string.IsNullOrWhiteSpace(txtPublishers.Text) ? "—" : txtPublishers.Text);
                }
            }
            catch
            {
                // ignore: giữ snapshot từ item nếu RAWG lỗi
            }
        }
        private void RenderRightPanel(TopSuggestionItem? it)
        {
            if (it == null)
            {
                imgCover.Source = null;
                txtTitle.Text = "";
                txtVotes.Text = "";
                txtDesc.Text = "";
                btnVote.IsEnabled = btnUnvote.IsEnabled = false;
                return;
            }

            txtTitle.Text = it.title;
            txtVotes.Text = $"Votes: {it.votes}";
            // Top list không có mô tả dài -> để trống
            txtDesc.Text = "";

            SetImage(imgCover, it.rawgBackgroundImg);
        }
        private void UpdateDetail(TopSuggestionItem it) => RenderRightPanel(it);

        private static void SetImage(Image target, string? url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url)) { target.Source = null; return; }
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(url, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmp.EndInit();
                bmp.Freeze();
                target.Source = bmp;
            }
            catch
            {
                target.Source = null;
            }
        }
        private void UpdateButtons(TopSuggestionItem? it)
        {
            if (it == null) { btnVote.IsEnabled = btnUnvote.IsEnabled = false; return; }
            btnVote.IsEnabled = !it.hasVoted;
            btnUnvote.IsEnabled = it.hasVoted;
        }


        private async void Vote_Click(object sender, RoutedEventArgs e)
        {
            if (_cur == null || _busy) return;
            _busy = true;
            btnVote.IsEnabled = btnUnvote.IsEnabled = false;

            try
            {
                var res = await _api.VoteAsyncEx(_currentUser.UserId, _cur.id);
                if (res.added)
                {
                    // Cập nhật data + resort an toàn
                    using (View.DeferRefresh())
                    {
                        _cur.votes += 1;
                        _cur.hasVoted = true;
                    }
                    txtVotes.Text = $"Votes: {_cur.votes}";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "API"); }
            finally
            {
                _busy = false;

                // BỎ CHỌN + trả focus về list để chọn item khác ngay
                lv.SelectedItem = null;
                Keyboard.ClearFocus();
                lv.Focus();
                Mouse.Capture(null);

                UpdateButtons(_cur);
            }
        }

        private async void Unvote_Click(object sender, RoutedEventArgs e)
        {
            if (_cur == null || _busy) return;
            _busy = true;
            btnVote.IsEnabled = btnUnvote.IsEnabled = false;

            try
            {
                var res = await _api.UnvoteAsyncEx(_currentUser.UserId, _cur.id);
                if (res.removed && _cur.votes > 0)
                {
                    using (View.DeferRefresh())
                    {
                        _cur.votes -= 1;
                        _cur.hasVoted = false;
                    }
                    txtVotes.Text = $"Votes: {_cur.votes}";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "API"); }
            finally
            {
                _busy = false;

                lv.SelectedItem = null;
                Keyboard.ClearFocus();
                lv.Focus();
                Mouse.Capture(null);

                UpdateButtons(_cur);
            }
        }

    }
}
