using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetroTrack.Models;
using RetroTrack.Services;

namespace RetroTrack.Controllers
{
    public class UserGameCollectionsController : Controller
    {
        private readonly ApiRestClientService _apiClient;

        public UserGameCollectionsController(ApiRestClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var collections = await _apiClient.GetUserGameCollectionsAsync();
            return View(collections);
        }

        public async Task<IActionResult> Details(int id)
        {
            var collection = await _apiClient.GetUserGameCollectionByIdAsync(id);
            if (collection == null) return NotFound();
            return View(collection);
        }

        public async Task<IActionResult> Create()
        {
            var games = await _apiClient.GetGamesAsync();
            ViewBag.Games = new SelectList(games, "Id", "Title");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserGameCollection collection)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.AddUserGameCollectionAsync(collection);
                return RedirectToAction(nameof(Index));
            }

            // Si hay error, volvemos a cargar juegos
            var games = await _apiClient.GetGamesAsync();
            ViewBag.Games = new SelectList(games, "Id", "Title", collection.GameId);
            return View(collection);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var collection = await _apiClient.GetUserGameCollectionByIdAsync(id);
            if (collection == null) return NotFound();

            var games = await _apiClient.GetGamesAsync();
            ViewBag.Games = new SelectList(games, "Id", "Title", collection.GameId);
            return View(collection);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserGameCollection collection)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.UpdateUserGameCollectionAsync(collection);
                return RedirectToAction(nameof(Index));
            }

            var games = await _apiClient.GetGamesAsync();
            ViewBag.Games = new SelectList(games, "Id", "Title", collection.GameId);
            return View(collection);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var collection = await _apiClient.GetUserGameCollectionByIdAsync(id);
            if (collection == null) return NotFound();
            return View(collection);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteUserGameCollectionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
