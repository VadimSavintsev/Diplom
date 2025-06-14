namespace Diplom
{
    public class UpdateEmployeeController : EmployeeController
    {
        private readonly EmployeeModel model;
        private readonly int employeeId;

        public UpdateEmployeeController(EmployeeModel model, int employeeId)
        {
            this.model = model;
            this.employeeId = employeeId;
        }

        public void Handle(string lastName, string firstName, string middleName,
                          string passportData, string inn, string snils,
                          string phoneNumber, string email, string login, string password)
        {
            EmployeeController.ValidateData(lastName, firstName, middleName,
                                           passportData, inn, snils,
                                           phoneNumber, email, login, password);

            model.UpdateEmployee(employeeId, lastName, firstName, middleName,
                               passportData, inn, snils,
                               phoneNumber, email, login, password);
        }
    }
}
