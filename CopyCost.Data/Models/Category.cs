using CommunityToolkit.Mvvm.ComponentModel;

namespace CopyCost.Core.Models;

public partial class Category : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private List<Payment> _payments = new();
}