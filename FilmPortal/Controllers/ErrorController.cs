using Microsoft.AspNetCore.Mvc;

namespace FilmPortal.Controllers
{
    /// <summary>
    /// Exeption Handling for production and other enviroments (Development is exepted)
    /// </summary>
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Exeption action
        /// </summary>
        /// <returns></returns>
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() =>
        Problem($"You make incorrect action. Response:{ControllerContext.HttpContext.Response}", nameof(ControllerContext.HttpContext.Response), 500, "Error");


        /// <summary>
        /// Metod for checking HandleError().
        /// </summary>
        /// <remarks>
        /// - Change Development enviroment on other.
        /// - Comment atribute "ApiExploreSetting"
        /// </remarks>
        /// <returns></returns>
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("Throw")]
        public IActionResult Throw() => throw new Exception("Sample exception.");
    }
}
