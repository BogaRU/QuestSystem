using Microsoft.AspNetCore.Mvc;
using QuestSystem.Application.DTOs;
using QuestSystem.Application.Interfaces;

namespace QuestSystem.API.Controllers
{
    /// <summary>
    /// Контроллер для управления квестами игроков.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerQuestsController : ControllerBase
    {
        private readonly IQuestService _questService;

        public PlayerQuestsController(IQuestService questService)
        {
            _questService = questService;
        }

        /// <summary>
        /// Принять квест для игрока.
        /// </summary>
        /// <param name="request">Запрос, содержащий идентификаторы игрока и квеста.</param>
        /// <returns>Подтверждение принятия квеста.</returns>
        /// <response code="200">Квест принят.</response>
        /// <response code="400">Некорректные идентификаторы или ошибка при принятии квеста.</response>
        [HttpPost("accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptQuest([FromBody] AcceptQuestRequestDto request)
        {
            if (request.PlayerId == Guid.Empty || request.QuestId == Guid.Empty)
                return BadRequest("Некорректные идентификаторы.");

            try
            {
                var playerQuest = await _questService.AcceptQuestAsync(request.PlayerId, request.QuestId);
                return Ok(playerQuest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновить прогресс выполнения квеста.
        /// </summary>
        /// <param name="playerQuestId">Идентификатор квеста игрока.</param>
        /// <param name="progressUpdate">Данные для обновления прогресса.</param>
        /// <returns>Обновленный прогресс квеста.</returns>
        /// <response code="200">Прогресс квеста успешно обновлен.</response>
        /// <response code="400">Ошибка обновления прогресса или неверные данные.</response>
        [HttpPatch("progress/{playerQuestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProgress(Guid playerQuestId, [FromBody] QuestProgressUpdateDto progressUpdate)
        {
            try
            {
                var updatedQuest = await _questService.UpdateQuestProgressAsync(playerQuestId, progressUpdate);
                return Ok(updatedQuest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Завершить квест.
        /// </summary>
        /// <param name="playerQuestId">Идентификатор квеста игрока.</param>
        /// <returns>Подтверждение успешного завершения квеста и получение награды.</returns>
        /// <response code="200">Квест успешно завершен.</response>
        /// <response code="400">Ошибка при завершении квеста или неверные данные.</response>
        [HttpPost("finish/{playerQuestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FinishQuest(Guid playerQuestId)
        {
            try
            {
                await _questService.FinishQuestAsync(playerQuestId);
                return Ok("Квест успешно завершен и награда получена.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
