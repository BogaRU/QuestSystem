using QuestSystem.Application.Interfaces;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestSystem.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task CreatePlayerAsync(Player player)
        {
            await _playerRepository.AddAsync(player);
            await _playerRepository.SaveChangesAsync();
        }

        public async Task DeletePlayerAsync(Guid playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                throw new ArgumentException("Игрок не найден.");

            await _playerRepository.Delete(player);
            await _playerRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _playerRepository.GetAllAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(Guid playerId)
        {
            return await _playerRepository.GetByIdAsync(playerId);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            await _playerRepository.Update(player);
            await _playerRepository.SaveChangesAsync();
        }
    }
}
