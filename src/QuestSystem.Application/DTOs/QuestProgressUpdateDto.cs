namespace QuestSystem.Application.DTOs
{
    public class QuestProgressUpdateDto
    {
        public ICollection<Guid> CollectedItemIds { get; set; }
        public int? DefeatedMonsters { get; set; }
        public int? VisitedLocations { get; set; }
    }
}
