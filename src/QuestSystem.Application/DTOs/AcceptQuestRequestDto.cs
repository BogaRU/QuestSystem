namespace QuestSystem.Application.DTOs
{
    public class AcceptQuestRequestDto
    {
        public Guid PlayerId { get; set; }
        public Guid QuestId { get; set; }
    }
}
