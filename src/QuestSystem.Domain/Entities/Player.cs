using System;

namespace QuestSystem.Domain.Entities
{
    public class Player
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int Money { get; private set; }
        public int Experience { get; private set; }
        public ICollection<PlayerItem> PlayerItems { get; private set; }

        public ICollection<PlayerQuest> PlayerQuests { get; private set; }

        public Player(string name = "Anonymous", int level = 1, int money = 0, int experience = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            Level = level;
            Money = money;
            Experience = experience;
            PlayerQuests = new List<PlayerQuest>();
            PlayerItems = new List<PlayerItem>();
        }

        public async Task UpdateLevel(int newLevel)
        {
            if (newLevel < Level)
                throw new ArgumentException("Новый уровень не может быть ниже текущего.");

            Level = newLevel;
        }

        public async Task AddExperience(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Опыт не может быть отрицательным.");

            // Можно реализовать более сложную логику, здесь предположим, что каждые 100 опыта повышают уровень игрока на 1.
            Experience += amount;
            if (Experience >= 100)
            {
                Level += Experience / 100;
                Experience %= 100;
            }
        }

        public async Task AddMoney(int moneyAmount)
        {
            // Можно реализовать более сложную логику, например, обрабатывать ситуацию с отрицательным балансом и т.п.
            Money += moneyAmount;
        }

        public async Task AddItem(PlayerItem playerItem)
        {
            var existingItem = PlayerItems.FirstOrDefault(pi => pi.ItemId == playerItem.ItemId);
            if (existingItem != null)
            {
                await existingItem.AddQuantity(playerItem.Quantity);
            }
            else
            {
                PlayerItems.Add(playerItem);
            }

        }

        public async Task RemoveItem(Guid itemId, int quantity)
        {
            var existingItem = PlayerItems.FirstOrDefault(pi => pi.ItemId == itemId);
            if (existingItem == null || existingItem.Quantity < quantity)
                throw new InvalidOperationException("Недостаточно предметов для удаления.");

            await existingItem.RemoveQuantity(quantity);

            if (existingItem.Quantity == 0)
            {
                PlayerItems.Remove(existingItem);
            }
        }
    }
}
