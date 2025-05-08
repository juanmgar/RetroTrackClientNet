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

        public async Task<IActionResult> FetchDescription(int id)
        {
            var game = await _apiClient.GetGameByIdAsync(id);
            if (game == null) return NotFound();

            var codeID = "e6a0126080a14743825d61ecc3e5a349";
            var rawgUrl = $"https://api.rawg.io/api/games?search={Uri.EscapeDataString(game.Title)}&key={codeID}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(rawgUrl);

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Details", new { id });

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
            {
                var firstGame = results[0];
                if (firstGame.TryGetProperty("slug", out var slug))
                {
                    // Hacer otra llamada para obtener el detalle completo
                    var detailUrl = $"https://api.rawg.io/api/games/{slug.GetString()}?key={codeID}";
                    var detailResponse = await client.GetAsync(detailUrl);

                    if (detailResponse.IsSuccessStatusCode)
                    {
                        var detailJson = await detailResponse.Content.ReadAsStringAsync();
                        using var detailDoc = JsonDocument.Parse(detailJson);
                        var detailRoot = detailDoc.RootElement;

                        if (detailRoot.TryGetProperty("description_raw", out var description))
                        {
                            game.Description = description.GetString();
                            await _apiClient.UpdateGameAsync(game);
                        }
                    }
                }
            }

            return RedirectToAction("Details", new { id });
        }
    }
}
