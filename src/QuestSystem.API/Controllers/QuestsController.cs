using Microsoft.AspNetCore.Mvc;
using QuestSystem.Application.Interfaces;

namespace QuestSystem.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с квестами.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuestsController : ControllerBase
    {
        private readonly IQuestService _questService;

        public QuestsController(IQuestService questService)
        {
            _questService = questService;
        }

        /// <summary>
        /// Получение доступных квестов для игрока.
        /// </summary>
        /// <param name="playerId">Идентификатор игрока.</param>
        /// <returns>Список доступных квестов.</returns>
        /// <response code="200">Возвращает список доступных квестов.</response>
        /// <response code="400">Неверный идентификатор игрока.</response>
        [HttpGet("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableQuests([FromQuery] Guid playerId)
        {
            if (playerId == Guid.Empty)
                return BadRequest("Некорректный идентификатор игрока.");

            var quests = await _questService.GetAvailableQuestsAsync(playerId);
            return Ok(quests);
        }
    }
}
