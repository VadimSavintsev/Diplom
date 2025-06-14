namespace Diplom
{
    public class MainController
    {
        private EmployeeModel model;

        public MainController(EmployeeModel model)
        {
            this.model = model;
        }

        public EmployeeModel GetModel()
        {
            return model;
        }

        public int GetEmployeeIdByRowIndex(int rowIndex)
        {
            List<Employee> employees = model.GetEmployees(10, (rowIndex / 10));
            int localIndex = rowIndex % 10;
            return employees[localIndex].Id.Value;
        }

        public List<Employee> GetEmployees(int pageSize, int pageNum)
        {
            return model.GetEmployees(pageSize, pageNum);
        }

        public Employee GetEmployeeById(int employeeId)
        {
            return model.GetEmployeeById(employeeId);
        }

        public void AddEmployee(string lastName, string firstName, string middleName,
                              string passportData, string inn, string snils,
                              string phoneNumber, string email, string login, string password)
        {
            model.AddEmployee(lastName, firstName, middleName,
                             passportData, inn, snils,
                             phoneNumber, email, login, password);
        }

        public void UpdateEmployee(int employeeId, string lastName, string firstName, string middleName,
                                 string passportData, string inn, string snils,
                                 string phoneNumber, string email, string login, string password)
        {
            model.UpdateEmployee(employeeId, lastName, firstName, middleName,
                               passportData, inn, snils,
                               phoneNumber, email, login, password);
        }

        public void DeleteEmployee(int employeeId)
        {
            model.DeleteEmployee(employeeId);
        }

        public int GetEmployeeCount()
        {
            return model.GetCount();
        }
    }
}
