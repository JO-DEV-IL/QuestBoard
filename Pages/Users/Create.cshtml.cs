using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();

        public String successMessage = "";
        public String errorMessage = "";

        public String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            userInfo.userName = Request.Form["userName"];
            userInfo.email = Request.Form["email"];
            userInfo.password = Request.Form["password"];
            userInfo.confirm_password = Request.Form["confirm_password"];
            userInfo.class_specialty = Request.Form["class_specialty"];

            // Confirm password validation
            if (userInfo.password != userInfo.confirm_password)
            {
                errorMessage = "Passwords do not match. Please try again.";
            }

            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                        optionName.Value = "sql_Register";
                        command.Parameters.Add(optionName);

                        connection.Open();

                        command.Parameters.AddWithValue("@userName", userInfo.userName);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@password", userInfo.password);
                        command.Parameters.AddWithValue("@class", userInfo.class_specialty);

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    errorMessage = ex.Message;
                }

                // Clear the fields after submit
                userInfo.userName = "";
                userInfo.email = "";
                userInfo.password = "";
                userInfo.class_specialty = "";

                successMessage = "User has been created!";
            }
        }
    }
}
