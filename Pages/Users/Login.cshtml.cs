using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class LoginModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();

        public String errorMessage = "";

        public void OnPost()
        {
            userInfo.userName = Request.Form["userName"];
            userInfo.password = Request.Form["password"];

            if (userInfo.userName.Length == 0 || userInfo.password.Length == 0)
            {
                errorMessage = "All fields are required.";
                return;
            }
            else
            {
                try
                {
                    String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        String sql = "select * from [questboard_app].[dbo].[master_Users] where userName = @userName and password = @password";
                        String update = "update [questboard_app].[dbo].[master_Users] set is_active = 1 where userName = @userName and password = @password";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@userName", userInfo.userName);
                            command.Parameters.AddWithValue("@password", userInfo.password);
                            command.ExecuteNonQuery();

                            using (SqlCommand updateUser = new SqlCommand(update, connection))
                            {
                                updateUser.Parameters.AddWithValue("@userName", userInfo.userName);
                                updateUser.Parameters.AddWithValue("@password", userInfo.password);
                                updateUser.ExecuteNonQuery();
                            }

                            // Retrieve the dataset from the query
                            DataSet dataset = new DataSet();
                            SqlDataAdapter dataadapter = new SqlDataAdapter(command);
                            dataadapter.Fill(dataset);

                            // Boolean variable establishing if a table is returned and if a row is returned in that table
                            bool loginSuccessful = ((dataset.Tables.Count > 0) && (dataset.Tables[0].Rows.Count > 0));
                            String isAdmin = (string)dataset.Tables[0].Rows[0]["is_admin"];
                            String user = (string)dataset.Tables[0].Rows[0]["userName"];
                            int userID = (int)dataset.Tables[0].Rows[0]["id"];

                            if (loginSuccessful)
                            {
                                HttpContext.Session.SetString("userActive", user); // Username of active user
                                HttpContext.Session.SetInt32("userID", userID); // ID of active user

                                Response.Redirect("/Users/Loading");

                                if (isAdmin == "Admin")
                                {
                                    HttpContext.Session.SetString("isAdmin", isAdmin); // Only create this session variable if the user is an Admin
                                }
                            }
                            else
                            {
                                errorMessage = "Invalid credentials. Please try again.";
                                return;
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }
    }
}
