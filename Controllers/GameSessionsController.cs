using Microsoft.AspNetCore.Mvc;
using RetroTrack.Models;
using RetroTrack.Services;

namespace RetroTrack.Controllers
{
    public class GameSessionsController : Controller
    {
        private readonly ApiRestClientService _apiClient;
        private readonly IConfiguration _configuration;

        public GameSessionsController(ApiRestClientService apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _apiClient.GetGameSessionsAsync();
            var games = await _apiClient.GetGamesAsync();

            var sessionModels = sessions.Select(s => new GameSession
            {
                Id = s.Id,
                GameId = s.GameId,
                GameTitle = games.FirstOrDefault(g => g.Id == s.GameId)?.Title ?? "Desconocido",
                PlayerId = s.PlayerId,
                PlayedAt = s.PlayedAt,
                MinutesPlayed = s.MinutesPlayed,
                Screenshot = s.Screenshot
            }).ToList();

            return View(sessionModels);
        }
    }
}
