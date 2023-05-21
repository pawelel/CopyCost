namespace CopyCost.Dto;

public class CategorySummary
{
    public string Customer { get; set; } = string.Empty;
    public DateTime MonthYear { get; set; }
    public int TextCount { get; set; }
    public decimal PricePer1000 { get; set; }
}