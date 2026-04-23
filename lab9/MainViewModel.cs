using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace lab9;

public class MainViewModel : ObservableObject
{
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
    public MainViewModel()
    {
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
        Contacts.Add(new Contact(_name, _phone));
        Name = string.Empty;
        Phone = string.Empty;
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
            Contacts.Remove(_selectedContact);
    }
    private bool CanDeleteContact()
    {
        return _selectedContact != null;
    }
}
