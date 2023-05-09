using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
    public class UserInfo
    {
        public String userID;
        public String userName;
        public String email;
        public String password;
        public String class_specialty;
        public String level;
    }

    public class AdminPanelModel : PageModel
    {
        public List<UserInfo> listUsers = new List<UserInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM [questboard_app].[dbo].[master_Users]";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.userID = "" + reader.GetInt32(0);
                                userInfo.userName = reader.GetString(1);
                                userInfo.email = "" + reader.GetInt32(2);
                                userInfo.password = "" + reader.GetInt32(3);
                                userInfo.class_specialty = reader.GetString(4);
                                userInfo.level = "" + reader.GetInt32(5);

                                listUsers.Add(userInfo);
                            }
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