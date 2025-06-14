using Diplom;

public class EmployeeModel : EmployeeObservable
{
    private IEmployeeModel employeeRep;

    public EmployeeModel(IEmployeeModel employeeRep)
    {
        this.employeeRep = employeeRep;
    }

    public List<Employee> GetEmployees(int pageSize, int pageNum)
    {
        return employeeRep.Get_k_n_list(pageSize, pageNum);
    }

    public Employee GetEmployeeById(int employeeId)
    {
        return employeeRep.GetEmployeeById(employeeId);
    }

    public void AddEmployee(string lastName, string firstName, string middleName,
                          string passportData, string inn, string snils,
                          string phoneNumber, string email, string login, string password)
    {
        Employee newEmployee = new Employee(
            id: null,
            lastName: lastName,
            firstName: firstName,
            middleName: middleName,
            passportData: passportData,
            inn: inn,
            snils: snils,
            phoneNumber: phoneNumber,
            email: email,
            login: login,
            password: password
        );

        employeeRep.AddEmployee(newEmployee);
        NotifyObservers("add", newEmployee);
    }

    public void UpdateEmployee(int employeeId, string lastName, string firstName, string middleName,
                             string passportData, string inn, string snils,
                             string phoneNumber, string email, string login, string password)
    {
        Employee updatedEmployee = new Employee(
            id: employeeId,
            lastName: lastName,
            firstName: firstName,
            middleName: middleName,
            passportData: passportData,
            inn: inn,
            snils: snils,
            phoneNumber: phoneNumber,
            email: email,
            login: login,
            password: password
        );

        bool success = employeeRep.ReplaceEmployeeById(employeeId, updatedEmployee);
        if (!success)
        {
            throw new Exception($"Ошибка обновления данных работника с id: {employeeId}");
        }
        else
        {
            NotifyObservers("update", updatedEmployee);
        }
    }

    public void DeleteEmployee(int employeeId)
    {
        employeeRep.DeleteEmployeeById(employeeId);
        NotifyObservers("delete", employeeId);
    }

    public int GetCount()
    {
        return employeeRep.GetCount();
    }
}
