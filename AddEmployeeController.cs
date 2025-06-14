namespace Diplom
{
    public class AddEmployeeController : EmployeeController
    {
        private readonly EmployeeModel model;

        public AddEmployeeController(EmployeeModel model)
        {
            this.model = model;
        }

        public void Handle(string lastName, string firstName, string middleName,
                          string passportData, string inn, string snils,
                          string phoneNumber, string email, string login, string password)
        {
            EmployeeController.ValidateData(lastName, firstName, middleName,
                                          passportData, inn, snils,
                                          phoneNumber, email, login, password);

            model.AddEmployee(lastName, firstName, middleName,
                             passportData, inn, snils,
                             phoneNumber, email, login, password);
        }
    }
}
