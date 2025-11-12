namespace Hoshiko.Data.Entities;

public enum SectionType
{
    Content = 0,
    Question = 1
}


public class StageSection
{
    public int Id { get; set; }
    public SectionType Type { get; set; } = default!;
    public string Text { get; set; } = default!;
    

    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? OptionC { get; set; }
    public string? OptionD { get; set; }

    public string? CorrectAnswer { get; set; }

    public int StageId { get; set; }
    public Stage Stage { get; set; } = default!;
}