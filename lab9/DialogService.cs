using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace lab9.Services;

public class DialogService : IDialogService
{
    public void ShowInfo(string message, string title = "Информация")
    {
        MessageBox.Show(message, title);
    }
    public void ShowWarning(string message, string title = "Предупреждение")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    public bool ShowConfirmation(string message, string title = "Подтверждение")
    {
        MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
        return result == MessageBoxResult.Yes;
    }
}
