using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyCost.Core.Models;

namespace CopyCost.WPF.Categories;

public partial class CategoryViewModel : ObservableObject
{
    
    [ObservableProperty] private Category? _selectedCategory = null!;

    public ObservableCollection<Category> Categories { get; } = new()
    {
        new Category { Id = 1, Name = "Category 1", Description = "Description 1" },
        new Category { Id = 2, Name = "Category 2", Description = "Description 2" },
        new Category { Id = 3, Name = "Category 3", Description = "Description 3" }
    };
    public ICommand ListViewLostFocusCommand => new RelayCommand(() => SelectedCategory = null);
    
}