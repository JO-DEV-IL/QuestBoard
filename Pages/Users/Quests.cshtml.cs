using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuestBoard.Pages.Users
{
    public class QuestsModel : PageModel
    {
        public class UserQuests
        {
            public String title;
            public String description;
        }

        public List<UserQuests> listUserQuests = new List<UserQuests>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String getQuests = "select q.username,q.todoTitle,q.todoDescription from [questboard_app].[dbo].[user_quests] q join [questboard_app].[dbo].[users] u on u.username = q.username where q.username = @username";

                    using (SqlCommand command = new SqlCommand(getQuests, connection))
                    {
                        command.Parameters.AddWithValue("@username", HttpContext.Session.GetString("userActive"));
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserQuests userQuests = new UserQuests();
                                userQuests.title = reader.GetString(1);
                                userQuests.description = reader.GetString(2);

                                listUserQuests.Add(userQuests);
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch
            {

            }
        }

        public void OnPost(string todoTitle, string todoDescription)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("INSERT INTO [questboard_app].[dbo].[user_quests] (username, todoTitle, todoDescription) VALUES (@username, @title, @description)", connection))
                    {
                        command.Parameters.AddWithValue("@username", HttpContext.Session.GetString("userActive"));
                        command.Parameters.AddWithValue("@title", todoTitle);
                        command.Parameters.AddWithValue("@description", todoDescription);
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            // Refresh the same page to update quests
            Response.Redirect("/Users/Quests");
        }
    }
}