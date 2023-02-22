using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Data.SqlClient;

namespace QuestBoard.Pages.SqlHelpers
{
    public class GetStatsModel : PageModel
    {
        public class UserStats
        {
            public String level;
            public String class_specialty;
            public String hp;
            public String power;
            public String defense;
            public String luck;
            public String portrait;
        }

        public class UserEquipment
        {
            public String head;
            public String chest;
            public String arms;
            public String legs;
            public String feet;
            public String mainhand;
            public String offhand;
            public String accessory1;
            public String accessory2;
        }

        public List<UserStats> listUsersStats = new List<UserStats>();
        public List<UserEquipment> listEquipment = new List<UserEquipment>();

        public void OnGet()
        {
            string user = "";

            if (HttpContext.Session.GetString("userActive") != null)
            {
                user = HttpContext.Session.GetInt32("userID").ToString();
                GetStatsSQL(user);
                GetEquipmentSQL(user);
            }
        }

        public void OnPost()
        {
            //string selectedAvatar = Request.Form["avatar"];
            //string user = HttpContext.Session.GetInt32("userID").ToString();
            //Console.WriteLine(selectedAvatar);
            //Console.WriteLine(user);

            string connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseJO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False\"";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [questboard_app].[dbo].[user_stats] SET avatar_path = @avatar WHERE userID = @userID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //command.Parameters.AddWithValue("@avatar", selectedAvatar);
                        //command.Parameters.AddWithValue("@userID", user);
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Response.Redirect("/Users/Inventory");
        }

        public void GetStatsSQL(string user)
        {
            string sql = "select s.userID,u.username,s.level,s.class,s.hp,s.power,s.defense,s.luck,s.avatar_path from [questboard_app].[dbo].[user_stats] s join [questboard_app].[dbo].[users] u on u.userID = s.userID where u.userID = @user";
            using (SqlConnection connection = new SqlConnection("Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseJO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserStats userStats = new UserStats();
                            userStats.level = "" + reader.GetInt32(2);
                            userStats.class_specialty = reader.GetString(3);
                            userStats.hp = "" + reader.GetInt32(4);
                            userStats.power = "" + reader.GetInt32(5);
                            userStats.defense = "" + reader.GetInt32(6);
                            userStats.luck = "" + reader.GetInt32(7);
                            userStats.portrait = reader.GetString(8);

                            listUsersStats.Add(userStats);
                        }
                    }
                }
                connection.Close();
            }
        }

        public void GetEquipmentSQL(string user)
        {
            string sql = "select * from [questboard_app].[dbo].[user_equipment] where userID = @user";
            using (SqlConnection connection = new SqlConnection("Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseJO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserEquipment userEquipment = new UserEquipment();
                            userEquipment.head = reader.GetString(1);
                            userEquipment.chest = reader.GetString(2);
                            userEquipment.arms = reader.GetString(3);
                            userEquipment.legs = reader.GetString(4);
                            userEquipment.feet = reader.GetString(5);
                            userEquipment.mainhand = reader.GetString(6);
                            userEquipment.offhand = reader.GetString(7);
                            userEquipment.accessory1 = reader.GetString(8);
                            userEquipment.accessory2 = reader.GetString(9);

                            listEquipment.Add(userEquipment);
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
