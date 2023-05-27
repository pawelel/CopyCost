namespace CopyCost.Dto;

public class CustomerEarnings
{
    public string CustomerName { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Earnings { get; set; }
}