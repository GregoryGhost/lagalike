namespace Lagalike.Telegram.Controllers
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    using Lagalike.Telegram.Services;

    using Microsoft.AspNetCore.Mvc;

    public class WebhookController : ControllerBase
    {
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
            [FromBody] Update update)
        {
            await handleUpdateService.HandleUpdateAsync(update);
            return Ok();
        }
    }
}