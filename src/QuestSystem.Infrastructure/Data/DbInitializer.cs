using QuestSystem.Domain.Entities;
using QuestSystem.Domain.Enums;
using System;
using System.Linq;

namespace QuestSystem.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Players.Any())
                return;

            var players = new Player[]
            {
                new Player("Player1", 5),
                new Player("Player2", 10, 10),
                new Player("Player3", 50, 100, 10),
            };

            foreach (var player in players)
            {
                context.Players.Add(player);
            }

            var items = new Item[]
            {
                new Item("Железный шлем", ItemType.Armor),
                new Item("Лук", ItemType.Weapon),
                new Item("Зелье лечения", ItemType.Potion),
            };

            foreach (var item in items)
            {
                context.Items.Add(item);
            }

            var quests = new Quest[]
            {
                new Quest("Первый Квест", "Описание первого квеста", 1),
                new Quest("Второй Квест", "Описание второго квеста", 5),
            };

            foreach (var quest in quests)
            {
                context.Quests.Add(quest);
            }

            context.SaveChanges();
        }
    }
}
