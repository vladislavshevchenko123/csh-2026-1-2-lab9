using System;
using System.Collections.Generic;
using System.Text;

namespace lab9.Services;

public interface IDialogService
{
    public void ShowInfo(string message, string title = "Информация");
    public void ShowWarning(string message, string title = "Предупреждение");
    public bool ShowConfirmation(string message, string title = "Подтверждение");
}
