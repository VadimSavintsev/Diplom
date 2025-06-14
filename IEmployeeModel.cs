namespace Diplom
{
    public interface IEmployeeModel
    {
        Employee GetEmployeeById(int id);

        List<Employee> Get_k_n_list(int k, int n);

        void AddEmployee(Employee employee);

        bool ReplaceEmployeeById(int id, Employee newEmployee);

        void DeleteEmployeeById(int id);

        int GetCount();
    }
}
