using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace lab9;

public class Contact : ObservableObject
{
    private string _name = string.Empty;
    private string _phone = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            ValidateError ve = ValidateNameDetailed(value);
            if (ve != ValidateError.None)
                throw new ArgumentException($"Invalid name: {ve}");

            _name = value;
        }
    }
    public string Phone
    {
        get => _phone;
        set
        {
            ValidateError ve = ValidatePhoneDetailed(value);
            if (ve != ValidateError.None)
                throw new ArgumentException($"Invalid phone: {ve}");

            _phone = value;
        }
    }

    public Contact(string name, string phone)
    {
        _name = name;
        _phone = phone;
        ValidateError ve = ValidateDetailed();
        if (ve != ValidateError.None)
            throw new ArgumentException($"Invalid contact data: {ve}");
    }

    /// <summary>
    /// Проверка имени и телефона этого экземпляра на валидность без возвращения типа ошибки.
    /// </summary>
    /// <returns>Пройдена ли проверка</returns>
    public bool Validate() => ValidateDetailed() == ValidateError.None;
    /// <summary>
    /// Проверка имени и телефона этого экземпляра на валидность с возвращением типа ошибки.
    /// </summary>
    /// <returns>Тип ошибки валидации</returns>
    public ValidateError ValidateDetailed() => ValidateDetailed(_name, _phone);

    /// <summary>
    /// Проверка имени и телефона на валидность с возвращением типа ошибки.
    /// </summary>
    /// <param name="name">Имя для проверки</param>
    /// <param name="phone">Номер телефона для проверки</param>
    /// <returns>Тип ошибки валидации</returns>
    public static ValidateError ValidateDetailed(string name, string phone)
    {
        ValidateError ve = ValidateNameDetailed(name);
        if (ve != ValidateError.None)
            return ve;

        ve = ValidatePhoneDetailed(phone);
        if (ve != ValidateError.None)
            return ve;

        return ValidateError.None;
    }
    /// <summary>
    /// Проверка имени на валидность с возвращением типа ошибки.
    /// </summary>
    /// <param name="name">Имя для проверки</param>
    /// <returns>Тип ошибки валидации</returns>
    public static ValidateError ValidateNameDetailed(string name)
    {
        if (string.IsNullOrEmpty(name))
            return ValidateError.EmptyName;

        return ValidateError.None;
    }
    /// <summary>
    /// Проверка номера телефона на валидность с возвращением типа ошибки.
    /// </summary>
    /// <param name="phone">Номер телефона для проверки</param>
    /// <returns>Тип ошибки валидации</returns>
    public static ValidateError ValidatePhoneDetailed(string phone)
    {
        if (string.IsNullOrEmpty(phone))
            return ValidateError.EmptyPhone;

        //Допустим, что все коды регионов односимвольные
        if (phone.Length != 12)
            return ValidateError.InvalidPhoneLength;

        if (phone[0] != '+')
            return ValidateError.InvalidPhoneFormat;

        //Пытаемся распарсить оставшуюся часть номера как число, если не получается - значит там есть не цифры
        if (!long.TryParse(phone.Substring(1), out _))
            return ValidateError.InvalidSimbolsInPhone;

        return ValidateError.None;
    }

    /// <summary>
    /// Конвертирует ValidateError в строку для отображения пользователю.
    /// </summary>
    /// <param name="ve">Ошибка валидации</param>
    /// <returns>Строка с описанием ошибки</returns>
    public static string ValidateDetailedToString(ValidateError ve)
    {
        return ve switch
        {
            ValidateError.None => "Нет ошибки",
            ValidateError.EmptyName => "Имя не может быть пустым",
            ValidateError.EmptyPhone => "Номер телефона не может быть пустым",
            ValidateError.InvalidPhoneLength => "Номер телефона должен содержать 12 символов",
            ValidateError.InvalidPhoneFormat => "Номер телефона должен начинаться с '+'",
            ValidateError.InvalidSimbolsInPhone => "Номер телефона содержит недопустимые символы",
            _ => "Неизвестная ошибка"
        };
    }
}

/// <summary>
/// Тип ошибки в данных пользователя
/// </summary>
public enum ValidateError
{
    None,
    EmptyName,
    EmptyPhone,
    InvalidPhoneLength,
    InvalidPhoneFormat,
    InvalidSimbolsInPhone,
}
