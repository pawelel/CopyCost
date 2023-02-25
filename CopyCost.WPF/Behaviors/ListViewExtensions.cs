using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CopyCost.WPF.Behaviors;

public static class ListViewExtensions
{
    public static readonly DependencyProperty LostFocusCommandProperty =
        DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(ListViewExtensions), new PropertyMetadata(null, OnLostFocusCommandChanged));

    public static ICommand GetLostFocusCommand(ListView listView)
    {
        return (ICommand)listView.GetValue(LostFocusCommandProperty);
    }

    public static void SetLostFocusCommand(ListView listView, ICommand value)
    {
        listView.SetValue(LostFocusCommandProperty, value);
    }

    private static void OnLostFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListView listView)
        {
            listView.LostFocus -= OnLostFocus;
            if (e.NewValue is ICommand command)
            {
                listView.LostFocus += OnLostFocus;
            }
        }
    }

    private static void OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (sender is ListView listView && GetLostFocusCommand(listView) is { } command)
        {
            command.Execute(null);
        }
    }
}