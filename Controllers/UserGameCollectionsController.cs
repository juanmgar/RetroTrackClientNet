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
            var username = HttpContext.Session.GetString("username");
            var collections = await _apiClient.GetUserGameCollectionsByUserAsync(username);
            var games = await _apiClient.GetGamesAsync();

            // Crear diccionario para buscar títulos por ID
            var gameTitles = games.ToDictionary(g => g.Id, g => g.Title);

            //Rellenamos con el título
            var model = collections.Select(c => new UserGameCollection
            {
                Id = c.Id,
                User = c.User,
                GameId = c.GameId,
                GameTitle = gameTitles.ContainsKey(c.GameId) ? gameTitles[c.GameId] : "Desconocido"
            }).ToList();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var collection = await _apiClient.GetUserGameCollectionByIdAsync(id);
            if (collection == null) return NotFound();
            return View(collection);
        }

        public async Task<IActionResult> Create()
        {
            var username = HttpContext.Session.GetString("username");

            var games = await _apiClient.GetGamesAsync();
            var userCollections = await _apiClient.GetUserGameCollectionsByUserAsync(username);
            var ownedGameIds = userCollections.Select(c => c.GameId).ToHashSet();

            // Filtrar solo los juegos que no tenemos en la colección aún
            var availableGames = games.Where(g => !ownedGameIds.Contains(g.Id)).ToList();

            ViewBag.Games = new SelectList(availableGames, "Id", "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserGameCollection collection)
        {
                collection.User = HttpContext.Session.GetString("username");
                await _apiClient.AddUserGameCollectionAsync(collection);
                return RedirectToAction(nameof(Index));
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
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                collection.User = HttpContext.Session.GetString("username");
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
