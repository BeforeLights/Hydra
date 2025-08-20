using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HYDRA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
namespace HYDRA.BLL.Services

{
    public sealed class PendingItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int Votes { get; set; }
        public int? RawgId { get; set; }
        public string? RawgBackgroundImg { get; set; }
        public DateTime CreatedAt { get; set; }

        // Snapshot thêm cho admin xem:
        public DateOnly? RawgReleased { get; set; }
        public string? RawgPublishers { get; set; }
        public string? RawgGenres { get; set; }
        public string? RawgPlatforms { get; set; }

        public string? Description { get; set; }
    }

    public class SuggestionService
    {
        private readonly HydraContext _ctx;
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _json;

        public SuggestionService()
        {
            _ctx = new HydraContext();
            _http = new HttpClient();
            _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public SuggestionService(HydraContext ctx, HttpClient http)
        {
            _ctx = ctx;
            _http = http;
            _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // --- RAWG DTOs & helpers ---
        private record RawgSearchResponse(List<RawgShort> Results);
        private record RawgShort(int Id, string Name);
        private record RawgDetail(int Id,
                                  string Name,
                                  string Slug,
                                  string? Released,
                                  int? Metacritic,

                                  string? Background_Image,
                                  string? Description,
                                  string? Description_Raw,
                                  List<RawgPlatformEntry>? Platforms,
                                  List<RawgGenre>? Genres,
                                  List<RawgPublisher>? Publishers);
        private record RawgPlatformEntry(RawgPlatform Platform);
        private record RawgPlatform(int Id, string Name, string Slug);
        private record RawgGenre(int Id, string Name, string Slug);
        private record RawgPublisher(int Id, string Name, string Slug);
        public HydraContext Context => _ctx;
        public record RawgClientDetail(
           int Id,
           string Name,
           string? Description,
           string? BackgroundImage,
           string[] Platforms,
           string[] Genres,

           string[] Publishers,
           DateOnly? ReleaseDate);

        private async Task<RawgDetail?> GetRawgDetailAsync(int rawgId, string apiKey)
        {
            var url = $"https://api.rawg.io/api/games/{rawgId}?key={apiKey}";
            using var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return null;
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RawgDetail>(json, _json);
        }
        private async Task<int?> GetOrCreatePublisherIdAsync(string? rawgPublishers)
        {
            if (string.IsNullOrWhiteSpace(rawgPublishers)) return null;

            // Lấy tên publisher đầu tiên trong chuỗi snapshot (tách ; hoặc ,)
            var first = rawgPublishers
                .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(first)) return null;

            var existing = await _ctx.Companies.FirstOrDefaultAsync(c => c.Name == first);
            if (existing != null) return existing.CompanyId;

            var company = new Company { Name = first };
            _ctx.Companies.Add(company);
            await _ctx.SaveChangesAsync();   // cần Save để có CompanyId
            return company.CompanyId;
        }
        public async Task<RawgClientDetail?> GetRawgDetailForClientAsync(int rawgId, string apiKey)
        {
            var d = await GetRawgDetailAsync(rawgId, apiKey);
            if (d == null) return null;

            DateOnly? release = null;
            if (!string.IsNullOrWhiteSpace(d.Released) &&
                DateOnly.TryParse(d.Released, out var rd))
            {
                release = rd;
            }

            var desc = !string.IsNullOrWhiteSpace(d.Description_Raw) ? d.Description_Raw : d.Description;

            var platforms = d.Platforms != null
                ? d.Platforms.Select(p => p.Platform.Name).Where(n => !string.IsNullOrWhiteSpace(n)).ToArray()
                : Array.Empty<string>();

            var genres = d.Genres != null
                ? d.Genres.Select(g => g.Name).Where(n => !string.IsNullOrWhiteSpace(n)).ToArray()
                : Array.Empty<string>();

            var publishers = d.Publishers != null
                ? d.Publishers.Select(p => p.Name).Where(n => !string.IsNullOrWhiteSpace(n)).ToArray()
                : Array.Empty<string>();

            return new RawgClientDetail(
                d.Id,
                d.Name,
                desc,
                d.Background_Image,
                platforms,
                genres,
                publishers,
                release
            );
        }
        public Task<List<int>> GetVotedSuggestionIdsAsync(int userId, IEnumerable<int> suggestionIds)
        {
            var ids = suggestionIds.ToList();
            return _ctx.GameSuggestionVotes
                .Where(v => v.UserId == userId && ids.Contains(v.SuggestionId))
                .Select(v => v.SuggestionId)
                .ToListAsync();
        }
        public async Task<RawgClientDetail?> GetRawgDetailByNameForClientAsync(string name, string apiKey)
        {
            var list = await SearchRawgAsync(name, apiKey, 1);
            if (list.Count == 0) return null;
            int rid = (int)((dynamic)list[0]).rawgId;
            return await GetRawgDetailForClientAsync(rid, apiKey);
        }

        public async Task<List<object>> SearchRawgAsync(string query, string apiKey, int take = 10)
        {
            var url = $"https://api.rawg.io/api/games?search={Uri.EscapeDataString(query)}&page_size={take}&key={apiKey}";
            using var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return new();
            var json = await res.Content.ReadAsStringAsync();
            var s = JsonSerializer.Deserialize<RawgSearchResponse>(json, _json) ?? new(new());
            var result = new List<object>();
            foreach (var r in s.Results)
                result.Add(new { rawgId = r.Id, name = r.Name });
            return result;
        }

        private async Task<RawgDetail?> FindBestFromTitleAsync(string title, string apiKey)
        {
            var list = await SearchRawgAsync(title, apiKey, 1);
            if (list.Count == 0) return null;
            var first = (dynamic)list[0];
            int rid = (int)first.rawgId;
            return await GetRawgDetailAsync(rid, apiKey);
        }

        // --- Use cases ---
        public async Task<GameSuggestion> CreateSuggestionAsync(int createdByUserId, string title, string? platformText, string? description,
                                                                int? rawgId, string rawgApiKey)
        {
            if (await _ctx.Games.AnyAsync(g => g.Title == title))
                throw new InvalidOperationException("Tựa game này đã có trên cửa hàng.");

            if (await _ctx.GameSuggestions.AnyAsync(s => s.Title == title && s.Status == (byte)SuggestionStatus.Pending))
                throw new InvalidOperationException("Đã có đề xuất đang chờ duyệt cho tựa game này.");

            var s = new GameSuggestion
            {
                Title = title.Trim(),
                PlatformText = platformText?.Trim(),
                Description = description?.Trim(),
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.UtcNow,
                Status = (byte)SuggestionStatus.Pending
            };
            _ctx.GameSuggestions.Add(s);
            await _ctx.SaveChangesAsync();


            RawgDetail? detail = null;
            if (rawgId.HasValue) detail = await GetRawgDetailAsync(rawgId.Value, rawgApiKey);
            if (detail == null) detail = await FindBestFromTitleAsync(title, rawgApiKey);

            if (detail != null)
            {
                DateOnly? released = null;
                if (!string.IsNullOrWhiteSpace(detail.Released) &&
                    DateOnly.TryParse(detail.Released, out var d)) released = d;

                var desc = !string.IsNullOrWhiteSpace(detail.Description_Raw)
                    ? detail.Description_Raw
                    : detail.Description;

                var platformsStr = detail.Platforms != null
                    ? string.Join("; ", detail.Platforms.Select(p => p.Platform.Name).Where(n => !string.IsNullOrWhiteSpace(n)))
                    : null;

                var genresStr = detail.Genres != null
                    ? string.Join("; ", detail.Genres.Select(g => g.Name).Where(n => !string.IsNullOrWhiteSpace(n)))
                    : null;

                var publishersStr = detail?.Publishers != null
                    ? string.Join("; ", detail.Publishers.Select(g => g.Name).Where(n => !string.IsNullOrWhiteSpace(n)))
                    : null;


                s.RawgId = detail.Id;
                s.RawgSlug = detail.Slug;
                s.RawgName = detail.Name;
                s.RawgReleased = released;
                s.RawgMetacritic = detail.Metacritic;
                s.RawgBackgroundImg = detail.Background_Image;
                s.RawgPlatforms = platformsStr;
                s.RawgGenres = genresStr;
                s.RawgPublishers = publishersStr;
                if (string.IsNullOrWhiteSpace(s.Description))
                    s.Description = desc;

                _ctx.GameSuggestions.Update(s);
                await _ctx.SaveChangesAsync();
            }

            return s;
        }


        public async Task<bool> VoteAsync(int userId, int suggestionId)
        {
            var s = await _ctx.GameSuggestions.FindAsync(suggestionId)
                    ?? throw new KeyNotFoundException("Không tìm thấy đề xuất.");
            if ((SuggestionStatus)s.Status != SuggestionStatus.Pending)
                throw new InvalidOperationException("Đề xuất đã được xử lý.");

            // kiểm tra trùng THEO suggestion
            bool exists = await _ctx.GameSuggestionVotes
                .AnyAsync(v => v.SuggestionId == suggestionId && v.UserId == userId);
            if (exists) return (false);

            _ctx.GameSuggestionVotes.Add(new GameSuggestionVote
            {
                SuggestionId = suggestionId,
                UserId = userId,
                VotedAt = DateTime.UtcNow
            });

            try
            {
                await _ctx.SaveChangesAsync();
                return (true);
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                // Bị unique index chặn => coi như đã vote rồi
                return (false);
            }
        }
        private static bool IsUniqueViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                // 2601 = Cannot insert duplicate key row... with unique index
                // 2627 = Violation of UNIQUE KEY constraint
                return sqlEx.Number == 2601 || sqlEx.Number == 2627;
            }
            return false;
        }

        public async Task<bool> UnvoteAsync(int userId, int suggestionId)
        {
            var v = await _ctx.GameSuggestionVotes
                .SingleOrDefaultAsync(x => x.SuggestionId == suggestionId && x.UserId == userId);
            if (v == null) return false;
            _ctx.GameSuggestionVotes.Remove(v);
            await _ctx.SaveChangesAsync();
            return true;
        }


        public async Task<List<PendingItemDto>> GetTopPendingAsync(int take = 100)
        {
            return await _ctx.GameSuggestions
                .AsNoTracking()
                .Where(s => s.Status == (byte)SuggestionStatus.Pending)
                .Select(s => new PendingItemDto
                {
                    Id = s.SuggestionId,
                    Title = s.RawgName ?? s.Title,
                    Votes = s.Votes.Count(),
                    RawgId = s.RawgId,
                    RawgBackgroundImg = s.RawgBackgroundImg,
                    CreatedAt = s.CreatedAt,
                    RawgReleased = s.RawgReleased,
                    RawgPublishers = s.RawgPublishers
                })
                .OrderByDescending(x => x.Votes)
                .ThenBy(x => x.CreatedAt)
                .Take(take)
                .ToListAsync();
        }
        public async Task<List<PendingItemDto>> GetPendingAsync(int take)
        {
            var query = await _ctx.GameSuggestions
                .Where(s => s.Status.Equals("pending"))
                .OrderByDescending(s => s.CreatedAt)
                .Take(take)
                .Select(s => new PendingItemDto
                {
                    Id = s.SuggestionId,
                    Title = s.RawgName ?? s.Title,
                    Description = s.Description,
                    RawgBackgroundImg = s.RawgBackgroundImg,
                    RawgReleased = s.RawgReleased,
                    RawgPublishers = s.RawgPublishers,
                    RawgGenres = s.RawgGenres,
                    RawgPlatforms = s.RawgPlatforms,
                    CreatedAt = s.CreatedAt,

                    // 👇 Lấy count từ bảng votes thay vì dùng field trong Suggestion
                    Votes = _ctx.GameSuggestionVotes.Count(v => v.SuggestionId == s.SuggestionId)
                })
                .ToListAsync();

            return query;
        }


        public async Task ApproveAsync(int adminUserId, int suggestionId, string? note, decimal defaultPrice = 0m)
        {
            var s = await _ctx.GameSuggestions
                .Include(x => x.Votes)
                .SingleOrDefaultAsync(x => x.SuggestionId == suggestionId)
                ?? throw new KeyNotFoundException("Không tìm thấy đề xuất.");

            if ((SuggestionStatus)s.Status != SuggestionStatus.Pending)
                throw new InvalidOperationException("Đề xuất đã được xử lý.");

            var titleToUse = s.RawgName ?? s.Title;
            var existingGames = await _ctx.Games.SingleOrDefaultAsync(g => g.Title == titleToUse);

            int? publisherId = null;
            if (!string.IsNullOrWhiteSpace(s.RawgPublishers))
            {
                var firstPublisher = s.RawgPublishers
                    .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(firstPublisher))
                {
                    // so sánh không phân biệt hoa/thường
                    var existing = await _ctx.Companies
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == firstPublisher.ToLower());

                    if (existing != null)
                        publisherId = existing.CompanyId;

                    //  tự động tạo Company mới, mở comment 4 dòng dưới:
                    else
                    {
                        var company = new Company { Name = firstPublisher };
                        _ctx.Companies.Add(company);
                        await _ctx.SaveChangesAsync();
                        publisherId = company.CompanyId;
                    }
                }
            }

            if (existingGames != null)
            {
                if (publisherId.HasValue && existingGames.PublisherId == null)
                    existingGames.PublisherId = publisherId;
                existingGames.IsForSale = true;
                if (existingGames.Price <= 0 && defaultPrice > 0) existingGames.Price = defaultPrice;
                if (existingGames.ReleaseDate == null && s.RawgReleased != null)
                    existingGames.ReleaseDate = s.RawgReleased;
            }
            else
            {
                var g = new Game
                {
                    Title = titleToUse,
                    Description = s.Description ?? (s.RawgSlug != null ? $"Imported from RAWG: {s.RawgSlug}" : null),
                    CoverArtPath = s.RawgBackgroundImg,
                    ReleaseDate = s.RawgReleased,
                    IsForSale = true,
                    Price = defaultPrice,
                    PublisherId = publisherId
                };
                _ctx.Games.Add(g);
            }

            s.Status = (byte)SuggestionStatus.Approved;
            s.ApprovedByUserId = adminUserId;
            s.ApprovedAt = DateTime.UtcNow;
            s.ApprovedNote = note ?? "Approved";
            await _ctx.SaveChangesAsync();
        }

        public async Task RejectAsync(int adminUserId, int suggestionId, string? note)
        {
            var s = await _ctx.GameSuggestions.FindAsync(suggestionId)
                ?? throw new KeyNotFoundException("Không tìm thấy đề xuất.");
            if ((SuggestionStatus)s.Status != SuggestionStatus.Pending)
                throw new InvalidOperationException("Đề xuất đã được xử lý.");

            s.Status = (byte)SuggestionStatus.Rejected;
            s.ApprovedByUserId = adminUserId;
            s.ApprovedAt = DateTime.UtcNow;
            s.ApprovedNote = note ?? "Rejected";
            await _ctx.SaveChangesAsync();
        }
        public Task<HashSet<int>> GetVotedSuggestionIdsAsync(int userId)
        {
            return _ctx.GameSuggestionVotes
                .Where(v => v.UserId == userId)
                .Select(v => v.SuggestionId)
                .ToHashSetAsync();
        }
    }
}
