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
                MinutesPlayed = s.MinutesPlayed
            }).ToList();

            ViewBag.ApiBaseUrl = _configuration["ApiRestUrl"];

            return View(sessionModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var session = await _apiClient.GetGameSessionByIdAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameSession session)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.AddGameSessionAsync(session);
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var session = await _apiClient.GetGameSessionByIdAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameSession session)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.UpdateGameSessionAsync(session);
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var session = await _apiClient.GetGameSessionByIdAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteGameSessionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
