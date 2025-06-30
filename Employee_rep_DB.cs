using Diplom;
using Npgsql;
public class Employee_rep_DB : IEmployeeModel
{
    private readonly DBConnection _dbConnection;
    private readonly LoginGenerator _loginGenerator = new LoginGenerator();
    private readonly PasswordGenerator _passwordGenerator = new PasswordGenerator();
    private readonly PasswordHasher _passwordHasher = new PasswordHasher();
    private readonly SecureEncryption _encryptor = new SecureEncryption();;

    public Employee_rep_DB(DBConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    private NpgsqlConnection GetConnection()
    {
        return _dbConnection.GetConnection();

    }

    public Employee GetEmployeeById(int employeeId)
    {
        const string query = "SELECT * FROM Employees WHERE Id = @Id;";
        using var connection = GetConnection();

        try
        {
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", employeeId);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            return new Employee(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                lastName: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("LastName"))),
                firstName: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("FirstName"))),
                middleName: reader.IsDBNull(reader.GetOrdinal("MiddleName"))
                    ? null
                    : _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("MiddleName"))),
                passportData: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("PassportData"))),
                inn: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("Inn"))),
                snils: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("Snils"))),
                phoneNumber: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("PhoneNumber"))),
                email: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("Email"))),
                login: reader.GetString(reader.GetOrdinal("Login")),
                password: reader.GetString(reader.GetOrdinal("Password"))
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении сотрудника: {ex.Message}");
            return null;
        }
    }

    public List<Employee> Get_k_n_list(int k, int n)
    {
        const string query = @"
            SELECT Id, LastName, FirstName, MiddleName, 
                   PassportData, Inn, Snils, 
                   PhoneNumber, Email, Login, Password 
            FROM Employees 
            ORDER BY Id 
            LIMIT @Limit OFFSET @Offset;";

        using var connection = GetConnection();
        var employees = new List<Employee>();

        try
        {
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Limit", k);
            command.Parameters.AddWithValue("@Offset", n);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(new Employee(
                    id: reader.GetInt32(reader.GetOrdinal("Id")),
                    lastName: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("LastName"))),
                    firstName: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("FirstName"))),
                    middleName: reader.IsDBNull(reader.GetOrdinal("MiddleName"))
                        ? null
                        : _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("MiddleName"))),
                    passportData: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("PassportData"))),
                    inn: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("Inn"))),
                    snils: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("Snils"))),
                    phoneNumber: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("PhoneNumber"))),
                    email: _encryptor.Decrypt(reader.GetString(reader.GetOrdinal("Email"))),
                    login: reader.GetString(reader.GetOrdinal("Login")),
                    password: reader.GetString(reader.GetOrdinal("Password"))
                ));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении списка сотрудников: {ex.Message}");
        }

        return employees;
    }

    public void AddEmployee(Employee employee)
    {
        const string query = @"
            INSERT INTO Employees (
                LastName, FirstName, MiddleName, 
                PassportData, Inn, Snils, 
                PhoneNumber, Email, Login, Password
            ) VALUES (
                @LastName, @FirstName, @MiddleName, 
                @PassportData, @Inn, @Snils, 
                @PhoneNumber, @Email, @Login, @Password
            ) RETURNING Id;";

        employee.Login = _loginGenerator.GenerateLogin(8);
        var tempPassword = _passwordGenerator.GeneratePassword();
        employee.Password = _passwordHasher.HashPassword(tempPassword);

        using var connection = GetConnection();
        try
        {
            using var command = new NpgsqlCommand(query, connection);
            employee.Id = (int)command.ExecuteScalar();
            command.Parameters.AddWithValue("@LastName", _encryptor.Encrypt(employee.LastName));
            command.Parameters.AddWithValue("@FirstName", _encryptor.Encrypt(employee.FirstName));
            command.Parameters.AddWithValue("@MiddleName", (object)_encryptor.Encrypt(employee.MiddleName) ?? DBNull.Value);
            command.Parameters.AddWithValue("@PassportData", _encryptor.Encrypt(employee.PassportData));
            command.Parameters.AddWithValue("@Inn", _encryptor.Encrypt(employee.INN));
            command.Parameters.AddWithValue("@Snils", _encryptor.Encrypt(employee.SNILS));
            command.Parameters.AddWithValue("@PhoneNumber", _encryptor.Encrypt(employee.PhoneNumber));
            command.Parameters.AddWithValue("@Email", (object)_encryptor.Encrypt(employee.Email));

            command.Parameters.AddWithValue("@Login", employee.Login);
            command.Parameters.AddWithValue("@Password", employee.Password);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении сотрудника: {ex.Message}");
            throw;
        }
    }

    public bool ReplaceEmployeeById(int id, Employee newEmployee)
    {
        const string query = @"
            UPDATE Employees
            SET LastName = @LastName,
                FirstName = @FirstName,
                MiddleName = @MiddleName,
                PassportData = @PassportData,
                Inn = @Inn,
                Snils = @Snils,
                PhoneNumber = @PhoneNumber,
                Email = @Email,
                Login = @Login,
                Password = @Password
            WHERE Id = @Id;";

        using var connection = GetConnection();
        try
        {
            using var command = new NpgsqlCommand(query, connection);

            // Шифруем данные
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@LastName", _encryptor.Encrypt(newEmployee.LastName));
            command.Parameters.AddWithValue("@FirstName", _encryptor.Encrypt(newEmployee.FirstName));
            command.Parameters.AddWithValue("@MiddleName", (object)_encryptor.Encrypt(newEmployee.MiddleName) ?? DBNull.Value);
            command.Parameters.AddWithValue("@PassportData", _encryptor.Encrypt(newEmployee.PassportData));
            command.Parameters.AddWithValue("@Inn", _encryptor.Encrypt(newEmployee.INN));
            command.Parameters.AddWithValue("@Snils", _encryptor.Encrypt(newEmployee.SNILS));
            command.Parameters.AddWithValue("@PhoneNumber", _encryptor.Encrypt(newEmployee.PhoneNumber));
            command.Parameters.AddWithValue("@Email", (object)_encryptor.Encrypt(newEmployee.Email));

            command.Parameters.AddWithValue("@Login", newEmployee.Login);
            command.Parameters.AddWithValue("@Password",
                string.IsNullOrEmpty(newEmployee.Password)
                    ? GetCurrentPasswordHash(id)
                    : _passwordHasher.HashPassword(newEmployee.Password));

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении сотрудника: {ex.Message}");
            return false;
        }
    }

    private string GetCurrentPasswordHash(int employeeId)
    {
        const string query = "SELECT Password FROM Employees WHERE Id = @Id;";
        using var connection = GetConnection();

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", employeeId);
        return command.ExecuteScalar()?.ToString();
    }

    public void DeleteEmployeeById(int id)
    {
        const string query = "DELETE FROM Employees WHERE Id = @Id;";
        using var connection = GetConnection();

        try
        {
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении сотрудника: {ex.Message}");
            throw;
        }
    }

    public int GetCount()
    {
        const string query = "SELECT COUNT(*) FROM Employees;";
        using var connection = GetConnection();

        try
        {
            using var command = new NpgsqlCommand(query, connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении количества сотрудников: {ex.Message}");
            return -1;
        }
    }
}
