using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CopyCost.Core.Models;

namespace CopyCost.WPF.Customers;

public partial class CustomersViewModel : ObservableObject
{
    [ObservableProperty] private Customer _selectedCustomer = null!;

    public ObservableCollection<Customer> Customers { get; } = new()
    {
        new Customer { Id = 1, Title = "Customer 1", Description = "Description 1" },
        new Customer { Id = 2, Title = "Customer 2", Description = "Description 2" },
        new Customer { Id = 3, Title = "Customer 3", Description = "Description 3" }
    };

    [ObservableProperty] private string _newCustomerTitle = string.Empty;
    [ObservableProperty] private string _newCustomerDescription = string.Empty;
    [ObservableProperty] private string _editCustomerTitle = string.Empty;
    [ObservableProperty] private string _editCustomerDescription = string.Empty;
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
        EditCustomerTitle = SelectedCustomer.Title;
        EditCustomerDescription = SelectedCustomer.Description;
    }

    [RelayCommand]
    private void Visible()
    {
        DisplayButton = Visibility.Visible;
    }

    [RelayCommand]
    private void AddCustomer()
    {
        Customers.Add(new Customer
        {
            Id = Customers.Count + 1,
            Title = NewCustomerTitle,
            Description = NewCustomerDescription
        });
        IsAddPopupOpen = false;
    }

    [RelayCommand]
    private void EditCustomer()
    {
        if (!(EditCustomerTitle.Length > 3)) return;
        if (Customers.Any(c => c.Title.Equals(EditCustomerTitle, StringComparison.InvariantCultureIgnoreCase)))
        {
            IsEditPopupOpen = false; // Hide the popup
            MessageBox.Show("A customer with the same Title already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            IsEditPopupOpen = true; // Show the popup again
            return;
        }
        SelectedCustomer.Title = EditCustomerTitle;
        SelectedCustomer.Description = EditCustomerDescription;
        IsEditPopupOpen = false;
        MessageBox.Show("Customer edited successfully");
    }

    [RelayCommand]
    private void DeleteCustomer()
    {
        Customers.Remove(SelectedCustomer);
    }

    [RelayCommand]
    private void ClosePopup()
    {
        IsAddPopupOpen = false;
        IsEditPopupOpen = false;
    }
}