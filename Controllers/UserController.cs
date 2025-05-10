using Microsoft.AspNetCore.Mvc;

namespace RetroTrack.Controllers
{
    public class UserController : Controller
    {
        private readonly ApiSoapClientService _soapClient;

        public UserController(ApiSoapClientService soapClient)
        {
            _soapClient = soapClient;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var result = await _soapClient.RegisterUserAsync(username, password);
            ViewBag.Message = result;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _soapClient.LoginAsync(username, password);

            if (token.StartsWith("ey"))
            {
                // Guardar el token en sesión
                HttpContext.Session.SetString("JWT", token);
                HttpContext.Session.SetString("username", username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWT"); 
            return RedirectToAction("Index", "Home");
        }

    }
}
