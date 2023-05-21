namespace CopyCost.Dto;

public class CategoryByText
{
    public string Category { get; set; } = string.Empty;
    public int TextCount { get; set; }
    public int TotalCharacters { get; set; }
    public DateTime MonthYear { get; set; }
    public decimal Total { get; set; }
}