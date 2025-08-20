using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hydra.API.Services
{
    public sealed class ApiClient
    {
        // CHỈNH cổng API của bạn tại đây
        public static readonly ApiClient Default = new ApiClient("http://localhost:5153");

        private readonly HttpClient _http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        private readonly JsonSerializerOptions _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly string _base;

        public ApiClient(string baseUrl) => _base = baseUrl.TrimEnd('/');
        public sealed class ApiError : Exception
        {
            public int StatusCode { get; }
            public string Url { get; }
            public ApiError(int status, string url, string body)
                : base($"[{status}] {url}\n{body}") { StatusCode = status; Url = url; }
        }

        private async Task<T> Get<T>(string path)
        {
            var url = _base + path;
            var res = await _http.GetAsync(_base + path);
            var body = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode) throw new ApiError((int)res.StatusCode, url, body);
            return JsonSerializer.Deserialize<T>(body, _json)!;
        }

        private async Task<T> Post<T>(string path, object payload)
        {
            var url = _base + path;
            var json = JsonSerializer.Serialize(payload);
            var res = await _http.PostAsync(_base + path, new StringContent(json, Encoding.UTF8, "application/json"));
            var body = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode) throw new ApiError((int)res.StatusCode, url, body);
            if (typeof(T) == typeof(Unit)) return default!;
            return JsonSerializer.Deserialize<T>(body, _json)!;
        }

        // RAWG search
        public Task<List<RawgItem>> SearchRawgAsync(string q, int take = 10)
            => Get<List<RawgItem>>($"/api/rawg/search?q={Uri.EscapeDataString(q)}&take={take}");

        public Task<RawgDetail> GetRawgDetailAsync(int rawgId)
            => Get<RawgDetail>($"/api/rawg/{rawgId}");
        public Task<RawgDetail> GetRawgDetailByNameAsync(string q)
                    => Get<RawgDetail>($"/api/rawg/by-name?q={Uri.EscapeDataString(q)}");

        public Task<List<TopSuggestionItem>> GetTopSuggestionsAsync(int userId, int take = 50)
    => Get<List<TopSuggestionItem>>($"/api/suggestions/pending?take={take}&userId={userId}");
        // Suggestion flow
        public record SuggestionDto { public int SuggestionId { get; set; } public string Title { get; set; } = ""; }
        public Task<SuggestionDto> CreateSuggestionAsync(int userId, string title, string? platform, string? desc, int? rawgId)
            => Post<SuggestionDto>("/api/suggestions", new { userId, title, platformText = platform, description = desc, rawgId });

        public Task<Unit> VoteAsync(int userId, int suggestionId)
            => Post<Unit>($"/api/suggestions/{suggestionId}/vote", new { userId });

        public Task<Unit> UnvoteAsync(int userId, int suggestionId)
            => Post<Unit>($"/api/suggestions/{suggestionId}/unvote", new { userId });

        public Task<List<PendingItem>> GetPendingAsync(int? userId = null, int take = 100)
        {
            var qp = userId.HasValue ? $"?take={take}&userId={userId}" : $"?take={take}";
            return Get<List<PendingItem>>($"/api/suggestions/pending{qp}");
        }

        public Task<Unit> ApproveAsync(int suggestionId, int adminUserId, string? note, decimal defaultPrice)
            => Post<Unit>($"/api/suggestions/{suggestionId}/approve", new { adminUserId, note, defaultPrice });

        public Task<Unit> RejectAsync(int suggestionId, int adminUserId, string? note)
            => Post<Unit>($"/api/suggestions/{suggestionId}/reject", new { adminUserId, note });
        public async Task<List<TopSuggestionItem>> GetTopSuggestionsAsync(int take = 100)
        {
            try
            {
                return await Get<List<TopSuggestionItem>>($"/api/suggestions/top?take={take}");
            }
            catch (ApiError)
            {
                // fallback nếu bạn chỉ còn /pending
                return await Get<List<TopSuggestionItem>>($"/api/suggestions/pending?take={take}");
            }
        }
        public Task<VoteResult> VoteAsyncEx(int userId, int suggestionId)
    => Post<VoteResult>($"/api/suggestions/{suggestionId}/vote", new { userId });

        public Task<VoteResult> UnvoteAsyncEx(int userId, int suggestionId)
            => Post<VoteResult>($"/api/suggestions/{suggestionId}/unvote", new { userId });
        public sealed class Unit { }
    }

    // DTOs map với API

    public record RawgItem
    {
        public int rawgId { get; set; }
        public string name { get; set; } = "";
        public string? rawgBackgroundImg { get; set; }
    }
    public record RawgDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? BackgroundImage { get; set; }
        public string[]? Platforms { get; set; }
        public string[]? Genres { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string[]? Publishers { get; set; }
    }


    public record SuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? BackgroundImage { get; set; }
        public string[]? Platforms { get; set; }
        public string[]? Genres { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public string[]? Released { get; set; }

    }

    public record PendingItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int Votes { get; set; }
        public int? RawgId { get; set; }
        public string? RawgBackgroundImg { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RawgReleased { get; set; }
        public string? RawgPublishers { get; set; }
        public bool HasVoted { get; set; }
    }

    public class TopSuggestionItem : INotifyPropertyChanged
    {
        int _id;
        string _title = "";
        int _votes;
        string? _rawgBackgroundImg;
        int? _rawgId;
        DateTime _createdAt;
        string? _rawgPublishers;
        string? _rawgReleased;
        bool _hasVoted;

        public int id { get => _id; set => Set(ref _id, value); }
        public string title { get => _title; set => Set(ref _title, value); }

        public int votes { get => _votes; set => Set(ref _votes, value); }
        public bool hasVoted { get => _hasVoted; set => Set(ref _hasVoted, value); }

        public string? rawgBackgroundImg { get => _rawgBackgroundImg; set => Set(ref _rawgBackgroundImg, value); }
        public int? rawgId { get => _rawgId; set => Set(ref _rawgId, value); }
        public DateTime createdAt { get => _createdAt; set => Set(ref _createdAt, value); }
        public string? rawgPublishers { get => _rawgPublishers; set => Set(ref _rawgPublishers, value); }
        public string? rawgReleased { get => _rawgReleased; set => Set(ref _rawgReleased, value); }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
    public record VoteResult
    {
        public int suggestionId { get; set; }
        public bool added { get; set; }        // vote
        public bool removed { get; set; }      // unvote
        public string? message { get; set; }
    }

}
