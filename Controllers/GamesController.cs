using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RetroTrack.Models;
using RetroTrack.Services;

namespace RetroTrack.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApiRestClientService _apiClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public GamesController(ApiRestClientService apiClient, IHttpClientFactory httpClientFactory)
        {
            _apiClient = apiClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _apiClient.GetGamesAsync();
            return View(games);
        }
        public async Task<IActionResult> Details(int id)
        {
            var game = await _apiClient.GetGameByIdAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.AddGameAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _apiClient.GetGameByIdAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Game game)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.UpdateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var game = await _apiClient.GetGameByIdAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteGameAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
