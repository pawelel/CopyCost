using CommunityToolkit.Mvvm.ComponentModel;

namespace CopyCost.Core.Models;

public partial class Payment : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private int _customerId;
    [ObservableProperty] private int _categoryId;
    [ObservableProperty] private DateOnly _date;
    [ObservableProperty] private double _amount;
    [ObservableProperty] private int _per1000;
    [ObservableProperty] private double _total;
    [ObservableProperty] private Customer? _customer;
    [ObservableProperty] private Category? _category;
}