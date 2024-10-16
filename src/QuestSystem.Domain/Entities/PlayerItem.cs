using QuestSystem.Domain.Enums;
using QuestSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Entities
{
    public class PlayerItem
    {
        public Guid Id { get; private set; }
        public Guid PlayerId { get; private set; }
        public Guid ItemId { get; private set; }
        public int Quantity { get; private set; }

        public Player Player { get; private set; }
        public Item Item { get; private set; }

        public PlayerItem(Guid playerId, Guid itemId, int quantity = 1)
        {
            Id = Guid.NewGuid();
            PlayerId = playerId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public async Task AddQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Количество не может быть отрицательным.");
            Quantity += amount;
        }

        public async Task RemoveQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Количество не может быть отрицательным.");
            if (Quantity < amount)
                throw new InvalidOperationException("Недостаточно предметов для удаления.");
            Quantity -= amount;
        }
    }
}
