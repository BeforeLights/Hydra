using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Hydra.API.Services;

namespace Hydra.Gui
{
    public partial class RequestGameWindow : Window
    {
        private readonly ApiClient _api = ApiClient.Default;

        // TODO: Lấy từ tài khoản đăng nhập thật của bạn
        private readonly int CurrentUserId = 1;

        // item hiển thị ở list (không lộ rawgId)
        private class SearchItem
        {
            public int RawgId { get; init; }
            public string Name { get; init; } = "";
        }

        private RawgDetail? _selectedDetail;

        public RequestGameWindow()
        {
            InitializeComponent();
        }

        private async void SearchRawg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var q = (txtQuery.Text ?? "").Trim();
                if (q.Length < 2) { MessageBox.Show("Nhập ít nhất 2 ký tự."); return; }

                var list = await _api.SearchRawgAsync(q, 25);   // List<RawgItem> với rawgId/name
                                                                // HIỂN THỊ CHỈ TÊN
                lstResults.ItemsSource = list.Select(x => new { RawgId = x.rawgId, Name = x.name }).ToList();
            }
            catch (ApiClient.ApiError ex) { MessageBox.Show(ex.Message, "API"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void lstResults_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstResults.SelectedItem is null) return;

            // Lấy RawgId và Name từ anonymous item
            dynamic item = lstResults.SelectedItem;
            int rid = (int)item.RawgId;
            string name = (string)item.Name;

            try
            {
                _selectedDetail = await _api.GetRawgDetailAsync(rid);
            }
            catch (ApiClient.ApiError ex) when (ex.StatusCode == 404)
            {
                // fallback theo tên nếu id bị 404
                _selectedDetail = await _api.GetRawgDetailByNameAsync(name);
            }

            if (_selectedDetail == null) { MessageBox.Show("Không lấy được chi tiết game."); return; }
            BindDetail(_selectedDetail);
        }


        private void BindDetail(RawgDetail d)
        {
            txtTitle.Text = d.Name;
            txtDesc.Text = d.Description ?? "";

            txtPlatforms.Text = (d.Platforms is { Length: > 0 })
                ? string.Join(", ", d.Platforms!)
                : "—";

            txtGenres.Text = (d.Genres is { Length: > 0 })
                ? string.Join(", ", d.Genres!)
                : "—";

            txtPublishers.Text = (d.Publishers is { Length: > 0 })
                ? string.Join(", ", d.Publishers)
                : "—";
            txtReleaseDate.Text = d.ReleaseDate.HasValue
                ? d.ReleaseDate.Value.ToString("yyyy-MM-dd")
                : "—";

            imgCover.Source = null;
            if (!string.IsNullOrWhiteSpace(d.BackgroundImage))
            {
                try { imgCover.Source = new BitmapImage(new Uri(d.BackgroundImage)); }
                catch { /* ignore image errors */ }
            }
        }

        private async void Request_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDetail is null)
            {
                MessageBox.Show("Hãy double-click chọn 1 game trong danh sách trước.");
                return;
            }

            try
            {
                var sug = await _api.CreateSuggestionAsync(
                    userId: CurrentUserId,
                    title: _selectedDetail.Name,
                    platform: null,                     // không cần người dùng nhập
                    desc: _selectedDetail.Description,
                    rawgId: _selectedDetail.Id         // gửi lên server để snapshot
                );

                lblStatus.Text = $"✅ Requested: {_selectedDetail.Name} (Suggestion #{sug.SuggestionId})";
                // (tuỳ chọn) auto-vote user sau khi request:
                // await _api.VoteAsync(CurrentUserId, sug.SuggestionId);
            }
            catch (ApiClient.ApiError ex) { MessageBox.Show(ex.Message, "API", MessageBoxButton.OK, MessageBoxImage.Warning); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
