using Microsoft.AspNetCore.Mvc;
using RetroTrack.Models;
using RetroTrack.Services;

namespace RetroTrack.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApiRestClientService _apiClient;

        public StatisticsController(ApiRestClientService apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var stats = await _apiClient.GetGameStatisticsAsync();
            return View(stats);
        }
    }
}
