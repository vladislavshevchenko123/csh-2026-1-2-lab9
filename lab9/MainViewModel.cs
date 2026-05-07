using lab9.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace lab9;

public class MainViewModel : ObservableObject
{
    IDialogService _dialogService;

    // Коллекция контактов
    public ObservableCollection<Contact> Contacts { get; }
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            Set(ref _name, value);
            OnPropertyChanged(nameof(NameValidationError));
        }
    }
    private string _phone = string.Empty;
    public string Phone
    {
        get => _phone;
        set
        {
            Set(ref _phone, value); 
            OnPropertyChanged(nameof(PhoneValidationError));
        }
    }
    public string NameValidationError
    {
        get
        {
            ValidateError ve = Contact.ValidateNameDetailed(_name);
            if (ve == ValidateError.None)
                return string.Empty;
            return Contact.ValidateDetailedToString(ve);
        }
    }
    public string PhoneValidationError
    {
        get
        {
            ValidateError ve = Contact.ValidatePhoneDetailed(_phone);
            if (ve == ValidateError.None)
                return string.Empty;
            return Contact.ValidateDetailedToString(ve);
        }
    }

    private Contact? _selectedContact;
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set => Set(ref _selectedContact, value);
    }

    // Команды
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public MainViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
        Contacts = new ObservableCollection<Contact>();
        AddCommand = new RelayCommand(
            AddContact,
            () => CanAddContact());

        DeleteCommand = new RelayCommand(
            DeleteContact,
            () => CanDeleteContact());
    }
    private void AddContact()
    {
        if (Contacts.Any(c => c.Phone == _phone || c.Name == _name))
        {
            _dialogService.ShowWarning("Контакт с таким номером телефона или именем уже существует.", "Предупреждение");
            return;
        }

        Contacts.Add(new Contact(_name, _phone));
        Name = string.Empty;
        Phone = string.Empty;

        _dialogService.ShowInfo("Контакт успешно добавлен.", "Информация");
    }
    private bool CanAddContact()
    {
        ValidateError ve = Contact.ValidateDetailed(_name, _phone);
        if (ve == ValidateError.None)
            return true;

        return false;
    }
    private void DeleteContact()
    {
        if (_selectedContact != null)
        {
            if (_dialogService.ShowConfirmation("Вы уверены, что хотите удалить контакт?", "Подтверждение"))
            {
                Contacts.Remove(_selectedContact);
            }
        }
    }
    private bool CanDeleteContact()
    {
        return _selectedContact != null;
    }
}
