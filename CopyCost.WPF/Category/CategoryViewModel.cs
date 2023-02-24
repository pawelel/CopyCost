using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CopyCost.WPF.Category;

public partial class CategoryViewModel: ObservableObject
{
    [ObservableProperty] private ObservableCollection<string> _categories = new() { "Category 1", "Category 2", "Category 3" };
    
}