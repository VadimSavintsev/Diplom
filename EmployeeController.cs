using System.Text;

namespace Diplom
{
    public interface EmployeeController
    {
        public void Handle(string lastName, string firstName, string middleName,
                          string passportData, string inn, string snils,
                          string phoneNumber, string email, string login, string password);

        public static void ValidateData(string lastName, string firstName, string middleName,
                                      string passportData, string inn, string snils,
                                      string phoneNumber, string email, string login, string password)
        {
            StringBuilder errors = new StringBuilder();

            if (!Employee.ValidateName(lastName))
            {
                errors.AppendLine("Фамилия должна содержать только русские буквы и может включать дефис.");
            }
            if (!Employee.ValidateName(firstName))
            {
                errors.AppendLine("Имя должно содержать только русские буквы и может включать дефис.");
            }
            if (!string.IsNullOrEmpty(middleName) && !Employee.ValidateName(middleName))
            {
                errors.AppendLine("Отчество должно содержать только русские буквы и может включать дефис.");
            }
            if (!Employee.ValidatePassportData(passportData))
            {
                errors.AppendLine("Паспортные данные должны быть в формате: серия номер (10 цифр).");
            }
            if (!Employee.ValidateInn(inn))
            {
                errors.AppendLine("ИНН должен содержать 12 цифр для физического лица.");
            }
            if (!Employee.ValidateSnils(snils))
            {
                errors.AppendLine("СНИЛС должен содержать 11 цифр в формате: XXX-XXX-XXX YY.");
            }
            if (!Employee.ValidatePhoneNumber(phoneNumber))
            {
                errors.AppendLine("Телефон должен содержать цифры и быть записан в формате: +7(XXX)XXX-XX-XX.");
            }
            if (!Employee.ValidateEmail(email))
            {
                errors.AppendLine("Неверно введён адрес электронной почты.");
            }

            if (errors.Length > 0)
            {
                throw new ArgumentException(errors.ToString());
            }
        }
    }
}
