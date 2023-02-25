using System.Windows.Controls;
using CopyCost.WPF.Navigation;

namespace CopyCost.WPF.Categories;

public partial class CategoriesView : UserControl
{
    public CategoriesView()
    {
        InitializeComponent();
        var navigationItem = DataContext as NavigationItem;
        if (navigationItem?.ViewModel != null)
        {
            DataContext = navigationItem.ViewModel;
        }
    }
}