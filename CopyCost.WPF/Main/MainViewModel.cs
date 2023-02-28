using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CopyCost.WPF.Categories;
using CopyCost.WPF.Customers;
using CopyCost.WPF.Navigation;
using CopyCost.WPF.Payments;

namespace CopyCost.WPF.Main;

public partial class MainViewModel : ObservableObject
{
   private readonly CategoriesViewModel _viewModel = new();
   [ObservableProperty] private ObservableCollection<NavigationItem> _navigationItems;
    [ObservableProperty] private NavigationItem _selectedNavigationItem;

    public MainViewModel()
    {
        _navigationItems = new ObservableCollection<NavigationItem>
        {
            new() { Title = "Payments", Content = new PaymentsView() },
            new() { Title = "Customers", Content = new CustomersView() },
            new() { Title = "Categories", Content = new CategoriesView() { DataContext = _viewModel } 
        }};
        _selectedNavigationItem = NavigationItems[0];

        // Set the DataContext of the CategoriesView to an instance of CategoriesViewModel
        if (NavigationItems[2].Content is CategoriesView categoriesView)
        {
            categoriesView.DataContext = new CategoriesViewModel();
        }
        if (NavigationItems[1].Content is CustomersView customersView)
        {
            customersView.DataContext = new CustomersViewModel();
        }
    }
}
