using RetroTrack.Models;

namespace RetroTrack.Services
{
    public class ApiRestClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl = "http://localhost:5099/retrotrack/api"; // TODO: Ajustar URL Docker

        public ApiRestClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // -------------------------------
        // CRUD para Games
        // -------------------------------
        public async Task<List<Game>> GetGamesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Game>>($"{_baseApiUrl}/Games");
        }

        public async Task<Game> GetGameByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Game>($"{_baseApiUrl}/Games/{id}");
        }

        public async Task AddGameAsync(Game game)
        {
            await _httpClient.PostAsJsonAsync($"{_baseApiUrl}/Games", game);
        }

        public async Task UpdateGameAsync(Game game)
        {
            await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/Games/{game.Id}", game);
        }

        public async Task DeleteGameAsync(int id)
        {
            await _httpClient.DeleteAsync($"{_baseApiUrl}/Games/{id}");
        }

        // -------------------------------
        // CRUD para GameSessions
        // -------------------------------

        public async Task<List<GameSession>> GetGameSessionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GameSession>>($"{_baseApiUrl}/GameSessions");
        }

        public async Task<GameSession> GetGameSessionByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<GameSession>($"{_baseApiUrl}/GameSessions/{id}");
        }

        public async Task AddGameSessionAsync(GameSession session)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseApiUrl}/GameSessions", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateGameSessionAsync(GameSession session)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/GameSessions/{session.Id}", session);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteGameSessionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseApiUrl}/GameSessions/{id}");
            response.EnsureSuccessStatusCode();
        }

        // -------------------------------
        // CRUD para UserGameCollections
        // -------------------------------

        public async Task<List<UserGameCollection>> GetUserGameCollectionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserGameCollection>>($"{_baseApiUrl}/UserGameCollections");
        }

        public async Task<UserGameCollection> GetUserGameCollectionByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UserGameCollection>($"{_baseApiUrl}/UserGameCollections/{id}");
        }

        public async Task AddUserGameCollectionAsync(UserGameCollection collection)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseApiUrl}/UserGameCollections", collection);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserGameCollectionAsync(UserGameCollection collection)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/UserGameCollections/{collection.Id}", collection);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserGameCollectionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseApiUrl}/UserGameCollections/{id}");
            response.EnsureSuccessStatusCode();
        }

        // -------------------------------
        // Statistics
        // -------------------------------
        public async Task<List<GameStatistics>> GetGameStatisticsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GameStatistics>>($"{_baseApiUrl}/GameSessions/statistics");
        }


    }
}
