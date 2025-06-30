using System.Data;
using System.Diagnostics;
using Diplom;

namespace Diplom
{
    public class MainView : Form, EmployeeObserver
    {
        private MainController controller;
        private DataGridView employeeTable;
        private DataTable tableModel;
        private int currentPage = 1;
        private int pageSize = 5;

        public MainView(MainController controller)
        {
            this.controller = controller;

            tableModel = new DataTable();
            tableModel.Columns.Add("№");
            tableModel.Columns.Add("Фамилия");
            tableModel.Columns.Add("Имя");
            tableModel.Columns.Add("Отчество");
            tableModel.Columns.Add("Паспортные данные");
            tableModel.Columns.Add("ИНН");
            tableModel.Columns.Add("СНИЛС");
            tableModel.Columns.Add("Телефон");
            tableModel.Columns.Add("Email");

            Text = "Список работников";
            Size = new System.Drawing.Size(1000, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            employeeTable = new DataGridView
            {
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = tableModel,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = true,
                Size = new System.Drawing.Size(600, 300)
            };

            Controls.Add(employeeTable);

            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Right;
            Controls.Add(buttonPanel);

            Button addButton = new Button();
            addButton.Text = "Добавить";
            addButton.Click += (sender, e) => OpenAddEmployeeWindow();
            addButton.Size = new System.Drawing.Size(100, 30);
            addButton.Location = new Point(0, 100);
            buttonPanel.Controls.Add(addButton);

            Button updateButton = new Button();
            updateButton.Text = "Редактировать";
            updateButton.Click += (sender, e) => ViewEmployeeDetails();
            updateButton.Size = new System.Drawing.Size(100, 30);
            updateButton.Location = new Point(0, 150);
            buttonPanel.Controls.Add(updateButton);

            Button deleteButton = new Button();
            deleteButton.Text = "Удалить";
            deleteButton.Click += (sender, e) => DeleteEmployee();
            deleteButton.Size = new System.Drawing.Size(100, 30);
            deleteButton.Location = new Point(0, 200);
            buttonPanel.Controls.Add(deleteButton);

            Button prevButton = new Button();
            prevButton.Text = "Предыдущий";
            prevButton.Click += (sender, e) => PrevPage();
            prevButton.Size = new System.Drawing.Size(100, 30);
            prevButton.Location = new Point(0, 250);
            buttonPanel.Controls.Add(prevButton);

            Button nextButton = new Button();
            nextButton.Text = "Следующий";
            nextButton.Click += (sender, e) => NextPage();
            nextButton.Size = new System.Drawing.Size(100, 30);
            nextButton.Location = new Point(0, 300);
            buttonPanel.Controls.Add(nextButton);

            controller.GetModel().AddObserver(this);

            RefreshTable();
        }

        private void RefreshTable()
        {
            tableModel.Rows.Clear();

            List<Employee> employees = controller.GetEmployees(pageSize, currentPage-1);
            if (employees.Count == 0 && currentPage > 1)
            {
                currentPage--;
                RefreshTable();
                return;
            }

            int startIndex = currentPage;
            for (int i = 0; i < employees.Count; i++)
            {
                Employee employee = employees[i];
                tableModel.Rows.Add(
                [
                    startIndex + i,
                    employee.LastName,
                    employee.FirstName,
                    employee.MiddleName,
                    employee.PassportData,
                    employee.Inn,
                    employee.Snils,
                    employee.PhoneNumber,
                    employee.Email
                ]);
            }
        }

        private void PrevPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                RefreshTable();
            }
        }

        private void NextPage()
        {
            currentPage++;
            RefreshTable();
        }

        private void OpenAddEmployeeWindow()
        {
            this.Invoke((MethodInvoker)delegate
            {
                new AddUpdateEmployeeView(new AddEmployeeController(controller.GetModel()), "Добавить", null).Show();
            });
            RefreshTable();
        }

        private void ViewEmployeeDetails()
        {
            if (employeeTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работника для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedRow = employeeTable.SelectedRows[0].Index;
            int employeeId = controller.GetEmployeeIdByRowIndex((currentPage - 1) * pageSize + selectedRow);
            Employee employee = controller.GetEmployeeById(employeeId);

            if (employee != null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    new AddUpdateEmployeeView(new UpdateEmployeeController(controller.GetModel(), employeeId), "Редактировать", employee).Show();
                });
            }
            else
            {
                MessageBox.Show("Не удалось найти работника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            RefreshTable();
        }

        private void DeleteEmployee()
        {
            if (employeeTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работника для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedRow = employeeTable.SelectedRows[0].Index;
            int employeeId = controller.GetEmployeeIdByRowIndex((currentPage - 1) * pageSize + selectedRow);
            DialogResult confirm = MessageBox.Show("Вы уверены, что хотите удалить работника?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    controller.DeleteEmployee(employeeId);
                    MessageBox.Show("Работник успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка при удалении работника: " + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            RefreshTable();
        }

        public void Update(string action, object data)
        {
            Debug.WriteLine($"MainView получил действие: {action} с данными: {data}");
        }
    }
}
