using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class CreateModel : PageModel
    {
        // Global variable for Users Class
        public UserInfo userInfo = new UserInfo();

        // Global success/error messages
        public String successMessage = "";
        public String errorMessage = "";


        public void OnGet()
        {
            // Do nothing
        }

        public void OnPost()
        {
            // Grabs values from form POST request form
            userInfo.username = Request.Form["username"];
            userInfo.firstName = Request.Form["firstName"];
            userInfo.lastName = Request.Form["lastName"];
            userInfo.age = Request.Form["age"];
            userInfo.class_specialty = Request.Form["class_specialty"];
            userInfo.role = Request.Form["role"];
            userInfo.password = Request.Form["password"];

            // Validates all fields by checking if there is anything in those fields
            if (userInfo.username.Length == 0 || userInfo.firstName.Length == 0 || userInfo.lastName.Length == 0 || userInfo.age.Length == 0 || userInfo.class_specialty.Length == 0 || userInfo.password.Length == 0)
            {
                // Error message becomes the following string
                errorMessage = "All fields are required.";

                // Return the message
                return;
            }
            else
            {
                try
                {
                    String connectionString = "Data Source=LAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "INSERT into [questboard_app].[dbo].[users] (username,firstName,lastName,age,class,level,role,password) VALUES(@username,@firstName,@lastName,@age,@class,@level,@role,@password)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            // @ parameters in above sql query will be replaced with these userInfo values
                            command.Parameters.AddWithValue("@username", userInfo.username);
                            command.Parameters.AddWithValue("@firstName", userInfo.firstName);
                            command.Parameters.AddWithValue("@lastName", userInfo.lastName);
                            command.Parameters.AddWithValue("@age", userInfo.age);
                            command.Parameters.AddWithValue("@class", userInfo.class_specialty);
                            command.Parameters.AddWithValue("@password", userInfo.password);

                            // A newly created user's level is set to 1
                            command.Parameters.AddWithValue("@level", 1);

                            // Always set to Member role unless otherwise specified
                            command.Parameters.AddWithValue("@role", "Member");

                            // Execute query
                            command.ExecuteNonQuery();
                        }

                        // Close connection
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = ex.Message;
                    //return;
                    Console.WriteLine(ex);
                }

                // Clear the fields
                userInfo.username = "";
                userInfo.firstName = "";
                userInfo.lastName = "";
                userInfo.age = "";
                userInfo.class_specialty = "";
                userInfo.password = "";

                // Populate success message
                successMessage = "User has been created!";
            }
        }
    }
}
