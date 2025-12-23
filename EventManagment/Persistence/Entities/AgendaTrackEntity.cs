namespace Persistence.Entities;

public class AgendaTrackEntity
{
    public int Id { get; set; }
    public int AgendaItemId { get; set; }
    public string Title { get; set; } = null!;
    public string? Speaker { get; set; }
    public string? Room { get; set; }
    
    public virtual AgendaItemEntity AgendaItem { get; set; } = null!;

}