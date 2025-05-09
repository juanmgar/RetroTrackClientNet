using Microsoft.AspNetCore.Http;
using RetroTrack.Models;

namespace RetroTrack.Services
{
    public class ApiRestClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiRestClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWT");
            if (!string.IsNullOrEmpty(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        // -------------------------------
        // CRUD para Games
        // -------------------------------
        public async Task<List<Game>> GetGamesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Game>>("Games");
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Game>($"Games/{id}");
        }

        public async Task AddGameAsync(Game game)
        {
            await _httpClient.PostAsJsonAsync("Games", game);
        }

        public async Task UpdateGameAsync(Game game)
        {
            await _httpClient.PutAsJsonAsync($"Games/{game.Id}", game);
        }

        public async Task DeleteGameAsync(int id)
        {
            await _httpClient.DeleteAsync($"Games/{id}");
        }

        // -------------------------------
        // CRUD para GameSessions
        // -------------------------------
        public async Task<List<GameSession>> GetGameSessionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GameSession>>("GameSessions");
        }

        public async Task<GameSession> GetGameSessionByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<GameSession>($"GameSessions/{id}");
        }

        public async Task AddGameSessionAsync(GameSession session)
        {
            var response = await _httpClient.PostAsJsonAsync("GameSessions", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateGameSessionAsync(GameSession session)
        {
            var response = await _httpClient.PutAsJsonAsync($"GameSessions/{session.Id}", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteGameSessionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"GameSessions/{id}");
            response.EnsureSuccessStatusCode();
        }

        // -------------------------------
        // CRUD para UserGameCollections
        // -------------------------------
        public async Task<List<UserGameCollection>> GetUserGameCollectionsAsync()
        {
            AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<List<UserGameCollection>>("UserGameCollections");
        }

        public async Task<UserGameCollection> GetUserGameCollectionByIdAsync(int id)
        {
            AddJwtHeader();
            return await _httpClient.GetFromJsonAsync<UserGameCollection>($"UserGameCollections/{id}");
        }

        public async Task AddUserGameCollectionAsync(UserGameCollection collection)
        {
            AddJwtHeader();
            var response = await _httpClient.PostAsJsonAsync("UserGameCollections", collection);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserGameCollectionAsync(UserGameCollection collection)
        {
            AddJwtHeader();
            var response = await _httpClient.PutAsJsonAsync($"UserGameCollections/{collection.Id}", collection);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserGameCollectionAsync(int id)
        {
            AddJwtHeader();
            var response = await _httpClient.DeleteAsync($"UserGameCollections/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<UserGameCollection>> GetUserGameCollectionsByUserAsync(string username)
        {
            AddJwtHeader();
            var response = await _httpClient.GetAsync($"UserGameCollections/user/{username}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<UserGameCollection>>();
        }

        // -------------------------------
        // Obtener Statistics
        // -------------------------------
        public async Task<List<GameStatistics>> GetGameStatisticsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GameStatistics>>("GameStatistics");
        }
    }
}
