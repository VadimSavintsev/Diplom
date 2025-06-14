namespace Diplom
{
    public class AddUpdateEmployeeView : Form
    {
        private EmployeeController controller;

        private TextBox lastNameField;
        private TextBox firstNameField;
        private TextBox middleNameField;
        private TextBox passportDataField;
        private TextBox innField;
        private TextBox snilsField;
        private TextBox phoneNumberField;
        private TextBox emailField;
        private TextBox loginField;
        private TextBox passwordField;

        public AddUpdateEmployeeView(EmployeeController controller, string title, Employee employeeData)
        {
            this.controller = controller;

            Text = title;
            Size = new System.Drawing.Size(400, 450);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += (s, e) => this.Dispose();

            TableLayoutPanel tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.ColumnCount = 2;
            tableLayout.RowCount = 11;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            for (int i = 0; i < 11; i++)
            {
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            }

            tableLayout.Controls.Add(new Label { Text = "Фамилия:", AutoSize = true }, 0, 0);
            lastNameField = new TextBox();
            tableLayout.Controls.Add(lastNameField, 1, 0);

            tableLayout.Controls.Add(new Label { Text = "Имя:", AutoSize = true }, 0, 1);
            firstNameField = new TextBox();
            tableLayout.Controls.Add(firstNameField, 1, 1);

            tableLayout.Controls.Add(new Label { Text = "Отчество:", AutoSize = true }, 0, 2);
            middleNameField = new TextBox();
            tableLayout.Controls.Add(middleNameField, 1, 2);

            tableLayout.Controls.Add(new Label { Text = "Паспортные данные:", AutoSize = true }, 0, 3);
            passportDataField = new TextBox();
            tableLayout.Controls.Add(passportDataField, 1, 3);

            tableLayout.Controls.Add(new Label { Text = "ИНН:", AutoSize = true }, 0, 4);
            innField = new TextBox();
            tableLayout.Controls.Add(innField, 1, 4);

            tableLayout.Controls.Add(new Label { Text = "СНИЛС:", AutoSize = true }, 0, 5);
            snilsField = new TextBox();
            tableLayout.Controls.Add(snilsField, 1, 5);

            tableLayout.Controls.Add(new Label { Text = "Телефон:", AutoSize = true }, 0, 6);
            phoneNumberField = new TextBox();
            tableLayout.Controls.Add(phoneNumberField, 1, 6);

            tableLayout.Controls.Add(new Label { Text = "Email:", AutoSize = true }, 0, 7);
            emailField = new TextBox();
            tableLayout.Controls.Add(emailField, 1, 7);

            tableLayout.Controls.Add(new Label { Text = "Логин:", AutoSize = true }, 0, 8);
            loginField = new TextBox();
            tableLayout.Controls.Add(loginField, 1, 8);

            tableLayout.Controls.Add(new Label { Text = "Пароль:", AutoSize = true }, 0, 9);
            passwordField = new TextBox();
            passwordField.PasswordChar = '*';
            tableLayout.Controls.Add(passwordField, 1, 9);

            if (employeeData != null)
            {
                lastNameField.Text = employeeData.LastName;
                firstNameField.Text = employeeData.FirstName;
                middleNameField.Text = employeeData.MiddleName;
                passportDataField.Text = employeeData.PassportData;
                innField.Text = employeeData.INN;
                snilsField.Text = employeeData.SNILS;
                phoneNumberField.Text = employeeData.PhoneNumber;
                emailField.Text = employeeData.Email;
                loginField.Text = employeeData.Login;
                passwordField.Text = employeeData.Password;
            }

            Button actionButton = new Button { Text = title };
            actionButton.Click += (s, e) => HandleAction();
            actionButton.Size = new System.Drawing.Size(100, 30);
            tableLayout.Controls.Add(actionButton, 1, 10);
            tableLayout.SetColumnSpan(actionButton, 2);
            Controls.Add(tableLayout);
        }

        private void HandleAction()
        {
            try
            {
                string lastName = lastNameField.Text.Trim();
                string firstName = firstNameField.Text.Trim();
                string middleName = middleNameField.Text.Trim();
                string passportData = passportDataField.Text.Trim();
                string inn = innField.Text.Trim();
                string snils = snilsField.Text.Trim();
                string phoneNumber = phoneNumberField.Text.Trim();
                string email = emailField.Text.Trim();
                string login = loginField.Text.Trim();
                string password = passwordField.Text.Trim();

                controller.Handle(lastName, firstName, middleName, passportData, inn,
                                snils, phoneNumber, email, login, password);

                MessageBox.Show("Операция выполнена успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
