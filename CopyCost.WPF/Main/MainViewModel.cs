using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CopyCost.WPF.Categories;
using CopyCost.WPF.Customers;
using CopyCost.WPF.Navigation;
using CopyCost.WPF.Payments;

namespace CopyCost.WPF.Main;

public partial class MainViewModel : ObservableObject
{
   private readonly CategoryViewModel _viewModel = new();
    [ObservableProperty] private ObservableCollection<NavigationItem> _navigationItems = new();
    [ObservableProperty] private NavigationItem _selectedNavigationItem = new();

    public MainViewModel()
    {
        _navigationItems = new ObservableCollection<NavigationItem>
        {
            new() { Title = "Payments", Content = new PaymentsView() },
            new() { Title = "Customers", Content = new CustomersView() },
            new() { Title = "Categories", Content = new CategoriesView() { DataContext = _viewModel } 
        }};
        _selectedNavigationItem = NavigationItems[0];

        // Set the DataContext of the CategoriesView to an instance of CategoryViewModel
        if (NavigationItems[2].Content is CategoriesView categoriesView)
        {
            categoriesView.DataContext = new CategoryViewModel();
        }
    }
}
