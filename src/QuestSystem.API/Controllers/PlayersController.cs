using Microsoft.AspNetCore.Mvc;
using QuestSystem.Application.DTOs;
using QuestSystem.Application.Interfaces;
using QuestSystem.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace QuestSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Получить информацию об игроке по его ID.
        /// </summary>
        /// <param name="playerId">ID игрока</param>
        /// <returns>Информация об игроке</returns>
        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetPlayer(Guid playerId)
        {
            if (playerId == Guid.Empty)
                return BadRequest("Некорректный идентификатор игрока.");

            var player = await _playerService.GetPlayerByIdAsync(playerId);
            if (player == null)
                return NotFound("Игрок не найден.");

            return Ok(player);
        }

        /// <summary>
        /// Создать нового игрока.
        /// </summary>
        /// <param name="createPlayerDto">Данные нового игрока</param>
        /// <returns>Созданный игрок</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerDto createPlayerDto)
        {
            if (string.IsNullOrWhiteSpace(createPlayerDto.Name) || createPlayerDto.Level < 1)
                return BadRequest("Некорректные данные игрока.");

            var player = new Player(createPlayerDto.Name, createPlayerDto.Level);
            await _playerService.CreatePlayerAsync(player);
            return CreatedAtAction(nameof(GetPlayer), new { playerId = player.Id }, player);
        }
    }
}
