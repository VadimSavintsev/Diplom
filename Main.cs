using Diplom;
    public static class Program
    {
        public static void Main(string[] args)
        {

            string dbUrl = "postgres";
            string dbUser = "postgres";
            string dbPassword = "82571";
            string host = "localhost";
            string port = "5432";


            DBConnection dbConnection = DBConnection.GetInstance(dbUrl, dbUser, dbPassword, host, port);


            Employee_rep_DB repository = new Employee_rep_DB(dbConnection);


            EmployeeModel model = new EmployeeModel(repository);


            MainController controller = new MainController(model);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainView(controller));
        }
    }
