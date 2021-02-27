using DTO;
using Logic;
using LogicContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Website.Controllers
{
    public class SpotifyController : Controller
    {
        private ISpotifyLogic _logic;

        public SpotifyController(ISpotifyLogic logic)
        {
            _logic = logic;
        }
        public IActionResult Index()
        {
            /*
             * TODO - API Keys
             * Check if there's an API key and a User in session that we can use.
             * If we have both, we want to skip ahead to grabbing a list of playlists
             */

            LoginSession user = JsonConvert.DeserializeObject<LoginSession>(HttpContext.Session.GetString("user") ?? string.Empty);

            return View();
        }

        public IActionResult Callback([FromQuery] string code)
        {
            // TODO - Call the token endpoint with the code, save the resulting Tokens to the session, do a GetUser query and save the object, then redirect to Index
            LoginSession user = _logic.Login(code);
            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
            return RedirectToAction("Index");
        }
    }
}
