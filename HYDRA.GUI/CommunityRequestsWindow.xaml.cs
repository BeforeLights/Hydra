using Hydra.API.Services;
using HYDRA.DAL.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public CommunityRequestsWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            lv.ItemsSource = _view;
            Loaded += async (_, __) => await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                // QUAN TRỌNG: truyền userId để server set hasVoted đúng
                var list = await _api.GetTopSuggestionsAsync(_currentUser.UserId, 200);

                _all = new ObservableCollection<TopSuggestionItem>(
                    list.OrderByDescending(x => x.votes)
                        .ThenBy(x => x.title));

                ApplyFilter();

                // luôn hiển thị ngay 1 item
                if (_view.Count > 0 && lv.SelectedIndex < 0)
                    lv.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "API");
            }
        }

        private void ApplyFilter()
        {
            string q = txtSearch.Text?.Trim() ?? "";
            _view.Clear();
            foreach (var x in _all.Where(x => string.IsNullOrWhiteSpace(q) || x.title.Contains(q, StringComparison.OrdinalIgnoreCase)))
                _view.Add(x);
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

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                txtVotes.Text = $"Votes: {it.votes}";
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
            if (it == null)
            {
                btnVote.IsEnabled = btnUnvote.IsEnabled = false;
                return;
            }

            // hasVoted = user đã vote HOẶC là creator (server tính giúp)
            btnVote.IsEnabled = !it.hasVoted;
            btnUnvote.IsEnabled = it.hasVoted; // tùy policy: có thể cho unvote nếu không phải creator
        }

        private async void Vote_Click(object sender, RoutedEventArgs e)
        {
            if (_cur == null) return;
            btnVote.IsEnabled = btnUnvote.IsEnabled = false;
            try
            {
                var res = await _api.VoteAsyncEx(_currentUser.UserId, _cur.id);
                if (res.added)
                {
                    _cur.votes += 1;
                    _cur.hasVoted = true;
                    txtVotes.Text = $"Votes: {_cur.votes}";
                    lv.Items.Refresh();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "API"); }
            finally { UpdateButtons(_cur); }
        }

        private async void Unvote_Click(object sender, RoutedEventArgs e)
        {
            if (_cur == null) return;
            btnVote.IsEnabled = btnUnvote.IsEnabled = false;
            try
            {
                var res = await _api.UnvoteAsyncEx(_currentUser.UserId, _cur.id);
                if (res.removed )
                {
                    _cur.votes -= 1;
                    _cur.hasVoted = false;
                    txtVotes.Text = $"Votes: {_cur.votes}";
                    lv.Items.Refresh();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "API"); }
            finally { UpdateButtons(_cur); }
        }

    }
}
