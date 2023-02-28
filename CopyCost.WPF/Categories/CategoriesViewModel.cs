using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyCost.Core.Models;

namespace CopyCost.WPF.Categories;

public partial class CategoriesViewModel : ObservableObject
{
    [ObservableProperty] private Category _selectedCategory = null!;

    public ObservableCollection<Category> Categories { get; } = new()
    {
        new Category { Id = 1, Title = "Category 1", Description = "Description 1" },
        new Category { Id = 2, Title = "Category 2", Description = "Description 2" },
        new Category { Id = 3, Title = "Category 3", Description = "Description 3" }
    };

    [ObservableProperty] private string _newCategoryTitle = string.Empty;
    [ObservableProperty] private string _newCategoryDescription = string.Empty;
    [ObservableProperty] private string _editCategoryTitle = string.Empty;
    [ObservableProperty] private string _editCategoryDescription = string.Empty;
    [ObservableProperty] private bool _isAddPopupOpen;
    [ObservableProperty] private bool _isEditPopupOpen;
    [ObservableProperty] private Visibility _displayButton = Visibility.Collapsed;

    [RelayCommand]
    private void ShowAddPopup()
    {
        IsAddPopupOpen = true;
    }

    [RelayCommand]
    private void ShowEditPopup()
    {
        IsEditPopupOpen = true;
        EditCategoryTitle = SelectedCategory.Title;
        EditCategoryDescription = SelectedCategory.Description;
    }

    [RelayCommand]
    private void Visible()
    {
        DisplayButton = Visibility.Visible;
    }

    [RelayCommand]
    private void AddCategory()
    {
        Categories.Add(new Category
        {
            Id = Categories.Count + 1,
            Title = NewCategoryTitle,
            Description = NewCategoryDescription
        });
        IsAddPopupOpen = false;
    }

    [RelayCommand]
    private void EditCategory()
    {
        if (!(EditCategoryTitle.Length > 3)) return;
        if (Categories.Any(c => c.Title.Equals(EditCategoryTitle, StringComparison.InvariantCultureIgnoreCase)))
        {
            IsEditPopupOpen = false; // Hide the popup
            MessageBox.Show("A category with the same Title already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            IsEditPopupOpen = true; // Show the popup again
            return;
        }
        SelectedCategory.Title = EditCategoryTitle;
        SelectedCategory.Description = EditCategoryDescription;
        IsEditPopupOpen = false;
        MessageBox.Show("Category edited successfully");
    }

    [RelayCommand]
    private void DeleteCategory()
    {
        Categories.Remove(SelectedCategory);
    }

    [RelayCommand]
    private void ClosePopup()
    {
        IsAddPopupOpen = false;
        IsEditPopupOpen = false;
    }
}