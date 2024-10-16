using QuestSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestSystem.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; private set; }
        public ItemType Type { get => (ItemType)TypeId; set => TypeId = (int)value; }

        public Item(string name, ItemType type)
        {
            Type = type;
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}
