using System.Data.SqlClient;

string DNumber = null;
string MgrSSN = null;
string DName = null;
SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
builder.DataSource = "EMILSEBILSEPC";
builder.InitialCatalog = "Company";
builder.IntegratedSecurity = true;
while (true)
{
    Console.WriteLine("Pick a method");
    Console.WriteLine("0: Menu");
    Console.WriteLine("1: CreateDepartment(DName, MgrSSN)");
    Console.WriteLine("2: UpdateDepartmentName(DNumber, DName)");
    Console.WriteLine("3: UpdateDepartmentManager(DNumber, MgrSSN)");
    Console.WriteLine("4: DeleteDepartment(DNumber)");
    Console.WriteLine("5: GetDepartment(DNumber)");
    Console.WriteLine("6: GetAllDepartments");
    string methodChosen = Console.ReadLine();
    switch (methodChosen)
    {
        case "0":
            break;
        case "1":
            //CreateDepartment
            Console.WriteLine("Insert DName:");
            DName = Console.ReadLine();
            Console.WriteLine("Insert MgrSSN:");
            MgrSSN = Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("usp_CreateDepartment", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DName", DName);
                command.Parameters.AddWithValue("@MgrSSN", MgrSSN);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to get main menu");
            Console.ReadLine();
            break;
        case "2":
            //UpdateDepartmentName
            Console.WriteLine("Insert DNumber:");
            DNumber = Console.ReadLine();
            Console.WriteLine("Insert DName:");
            DName = Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("usp_UpdateDepartmentName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DName", DName);
                command.Parameters.AddWithValue("@DNumber", DNumber);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to get main menu");
            Console.ReadLine();
            break;
        case "3":
            //UpdateDepartmentManager
            Console.WriteLine("Insert DNumber:");
            DNumber = Console.ReadLine();
            Console.WriteLine("Insert MgrSSN:");
            MgrSSN = Console.ReadLine(); 
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("usp_UpdateDepartmentManager", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MgrSSN", MgrSSN);
                command.Parameters.AddWithValue("@DNumber", DNumber);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to get main menu");
            Console.ReadLine();
            break;
        case "4":
            //DeleteDepartment
            Console.WriteLine("Insert DNumber:");
            DNumber = Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("usp_DeleteDepartment", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DNumber", DNumber);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to get main menu");
            Console.ReadLine();
            break;
        case "5":
            //GetDepartment 
            Console.WriteLine("Insert DNumber:");
            DNumber = Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("usp_GetDepartment", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DNumber", DNumber);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Department Name: {reader["DName"]}, Department Number: {reader["DNumber"]}, MgrSSN: {reader["MgrSSN"]}, EmpCount: {reader["EmpCount"]}");
                }
                reader.Close();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to get main menu");
            Console.ReadLine();
            break;
        case "6":
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("usp_GetAllDepartments", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Department Name: {reader["DName"]}, Department Number: {reader["DNumber"]}, MgrSSN: {reader["MgrSSN"]}, MgrStartDate: {reader["MgrStartDate"]}, EmpCount: {reader["EmpCount"]}");
                }
                reader.Close();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to get main menu");
            Console.ReadLine();
            break;
        default:
            break;
    }
}