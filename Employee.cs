using System.Text.RegularExpressions;

public class Employee
{
    private int? id;
    private string lastName;
    private string firstName;
    private string middleName;
    private string passportData;
    private string inn;
    private string snils;
    private string phoneNumber;
    private string email;
    private string login;
    private string password;

    public int? Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public string LastName
    {
        get { return lastName; }
        set
        {
            if (!ValidateName(value))
            {
                throw new ArgumentException("Некорректно введена фамилия");
            }
            lastName = value;
        }
    }

    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (!ValidateName(value))
            {
                throw new ArgumentException("Некорректно введено имя");
            }
            firstName = value;
        }
    }

    public string MiddleName
    {
        get { return middleName; }
        set 
        {
            if (!ValidateName(value))
            {
                throw new ArgumentException("Некорректно введено отчество");
            }
            middleName = value; 
        }
    }

    public string PassportData
    {
        get { return passportData; }
        set
        {
            if (!ValidatePassportData(value))
            {
                throw new ArgumentException("Некорректные паспортные данные");
            }
            passportData = value;
        }
    }

    public string INN
    {
        get { return inn; }
        set
        {
            if (!ValidateInn(value))
            {
                throw new ArgumentException("Некорректный ИНН");
            }
            inn = value;
        }
    }

    public string SNILS
    {
        get { return snils; }
        set
        {
            if (!ValidateSnils(value))
            {
                throw new ArgumentException("Некорректный СНИЛС");
            }
            snils = value;
        }
    }

    public string PhoneNumber
    {
        get { return phoneNumber; }
        set
        {
            if (!ValidatePhoneNumber(value))
            {
                throw new ArgumentException("Некорректный телефон");
            }
            phoneNumber = value;
        }
    }

    public string Email
    {
        get { return email; }
        set
        {
            if (!ValidateEmail(value))
            {
                throw new ArgumentException("Некорректный email");
            }
            email = value;
        }
    }

    public string Password
    {
        get { return password; }
        set
        {
            password = value;
        }
    }
    public string Login
    {
        get { return login; }
        set
        {
            login = value;
        }
    }

    public Employee(int? id, string lastName, string firstName, string middleName, string passportData, string inn, string phoneNumber, string snils, string email, string login,string password)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        PassportData = passportData;
        INN = inn;
        PhoneNumber = phoneNumber;
        SNILS = snils;
        Email = email;
        Login = login;
        Password = password;
    }

    public static bool ValidateInn(string inn)
    {
        return !string.IsNullOrEmpty(inn) && Regex.IsMatch(inn, @"\d{10}");
    }

    public static bool ValidateSnils(string snils)
    {
        return !string.IsNullOrEmpty(snils) && Regex.IsMatch(snils, @"^\d{3}-\d{3}-\d{3} \d{2}$");
    }

    public static bool ValidateEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, @"(.+)@(.+)\.(.+)");
    }
    public static bool ValidatePhoneNumber(string phoneNumber)
    {
        return !string.IsNullOrEmpty(phoneNumber) && Regex.IsMatch(phoneNumber, @"\+7\(\d{3}\)\d{3}-\d{2}-\d{2}");
    }

    public static bool ValidateName(string name)
    {
        return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, "^[А-Яа-я-]+$");
    }
    public static bool ValidatePassportData(string passportData)
    {
        return !string.IsNullOrEmpty(passportData) && Regex.IsMatch(passportData, @"^\d{4}\s?\d{6}$");
    }
}
