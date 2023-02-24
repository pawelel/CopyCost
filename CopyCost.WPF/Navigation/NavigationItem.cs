namespace CopyCost.WPF.Navigation;

public class NavigationItem
{
    public string Title { get; init; } = string.Empty; 
    public object? Content { get; init; }
    public object? ViewModel { get; set; }
}