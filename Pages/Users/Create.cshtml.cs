using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();

        public String successMessage = "";
        public String errorMessage = "";

        public void OnGet()
        {
            // Do nothing
        }

        public void OnPost()
        {
            userInfo.userName = Request.Form["userName"];
            userInfo.firstName = Request.Form["firstName"];
            userInfo.lastName = Request.Form["lastName"];
            userInfo.age = Request.Form["age"];
            userInfo.class_specialty = Request.Form["class_specialty"];
            userInfo.password = Request.Form["password"];

            if (userInfo.userName.Length == 0 || userInfo.firstName.Length == 0 || userInfo.lastName.Length == 0 || userInfo.age.Length == 0 || userInfo.class_specialty.Length == 0 || userInfo.password.Length == 0)
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

                        String sql = 
                            // Handle user creation
                            "INSERT into [questboard_app].[dbo].[master_Users] (userName, firstName, lastName, age, class, level, role, password)"
                            + "VALUES (@userName, @firstName, @lastName, @age, @class, @level, @role, @password)"
                            
                            // Set user equips to default 0 (empty)
                            + "INSERT INTO [questboard_app].[dbo].[user_Equipment] (head, shoulders, chest, hands, legs, feet, mainhand, offhand, accessory1, accessory2)"
                            + "VALUES (0,0,0,0,0,0,0,0,0,0)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@userName", userInfo.userName);
                            command.Parameters.AddWithValue("@firstName", userInfo.firstName);
                            command.Parameters.AddWithValue("@lastName", userInfo.lastName);
                            command.Parameters.AddWithValue("@age", userInfo.age);
                            command.Parameters.AddWithValue("@class", userInfo.class_specialty);
                            command.Parameters.AddWithValue("@password", userInfo.password);
                            
                            command.Parameters.AddWithValue("@level", 1);
                            command.Parameters.AddWithValue("@role", "Member");

                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                // Clear the fields after submit
                userInfo.userName = "";
                userInfo.firstName = "";
                userInfo.lastName = "";
                userInfo.age = "";
                userInfo.class_specialty = "";
                userInfo.password = "";

                successMessage = "User has been created!";
            }
        }
    }
}
