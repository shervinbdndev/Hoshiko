namespace Hoshiko.Data.Entities;

public class Stage
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<StageSection> Sections { get; set; } = new List<StageSection>();
}