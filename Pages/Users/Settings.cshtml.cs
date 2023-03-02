using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static QuestBoard.Pages.SqlHelpers.GetStatsModel;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost()
        {
            UpdateAvatar();
        }

        public void UpdateAvatar()
        {
            try
            {
                string selectedAvatar = Request.Form["avatar"];
                string user = HttpContext.Session.GetInt32("userID").ToString();

                string sql = "update [questboard_app].[dbo].[user_Stats] set avatar = @avatar where userID = @userID";
                using (SqlConnection connection = new SqlConnection("Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseJO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", user);
                        command.Parameters.AddWithValue("@avatar", selectedAvatar);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Response.Redirect("/Users/Settings");
        }
    }
}
