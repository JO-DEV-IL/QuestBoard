using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class LoginModel : PageModel
    {
        // Global variable for Users Class
        public UserInfo userInfo = new UserInfo();

        // Global empty error message variable
        public String errorMessage = "";

        public void OnPost()
        {
            // Grab username and password values from Login form
            userInfo.username = Request.Form["username"];
            userInfo.password = Request.Form["password"];

            // Validate that fields are filled in
            if (userInfo.username.Length == 0 || userInfo.password.Length == 0)
            {
                errorMessage = "All fields are required.";
                return;
            }
            // If fields are not empty, run SQL operations
            else
            {
                // Try/catch block to single out any errors
                try
                {
                    String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        String sql = "select userID,username,password,role from [questboard_app].[dbo].[users] where username = @username and password = @password";
                        String update = "UPDATE [questboard_app].[dbo].[users] SET is_loggedin = 1 WHERE username = @username and password = @password";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@username", userInfo.username);
                            command.Parameters.AddWithValue("@password", userInfo.password);
                            command.ExecuteNonQuery();

                            using (SqlCommand updateUser = new SqlCommand(update, connection))
                            {
                                updateUser.Parameters.AddWithValue("@username", userInfo.username);
                                updateUser.Parameters.AddWithValue("@password", userInfo.password);
                                updateUser.ExecuteNonQuery();
                            }

                            // Retrieve the dataset from the query
                            DataSet dataset = new DataSet();
                            SqlDataAdapter dataadapter = new SqlDataAdapter(command);
                            dataadapter.Fill(dataset);

                            // Boolean variable establishing if a table is returned and if a row is returned in that table
                            bool loginSuccessful = ((dataset.Tables.Count > 0) && (dataset.Tables[0].Rows.Count > 0));
                            String isAdmin = (string)dataset.Tables[0].Rows[0]["role"];
                            String user = (string)dataset.Tables[0].Rows[0]["username"];
                            int userID = (int)dataset.Tables[0].Rows[0]["userID"];

                            if (loginSuccessful)
                            {
                                HttpContext.Session.SetString("userActive", user);
                                HttpContext.Session.SetInt32("userID", userID); // For inventory fetch

                                Response.Redirect("/Users/Loading");

                                if (isAdmin == "Admin")
                                {
                                    HttpContext.Session.SetString("isAdmin", isAdmin);
                                }
                            }
                            else
                            {
                                errorMessage = "Invalid credentials. Please try again.";
                                return;
                            }
                        }
                        // Close SQL connection
                        connection.Close();
                    }
                }
                // Catch any errors
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }
    }
}
