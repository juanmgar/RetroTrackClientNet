using Microsoft.AspNetCore.Mvc;
using RetroTrack.Models;
using RetroTrack.Services;

namespace RetroTrack.Controllers
{
    public class GameSessionsController : Controller
    {
        private readonly ApiRestClientService _apiClient;

        public GameSessionsController(ApiRestClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _apiClient.GetGameSessionsAsync();
            return View(sessions);
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
