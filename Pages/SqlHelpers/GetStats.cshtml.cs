using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Data;
using System.Data.SqlClient;

namespace QuestBoard.Pages.SqlHelpers
{
    public class GetStatsModel : PageModel
    {
        public class UserStats
        {
            public String portrait;
            public String level;
            public String class_specialty;
            public String hp;
            public String power;
            public String defense;
            public String luck;
        }

        public class UserEquipment
        {
            public String head;
            public String shoulders;
            public String chest;
            public String hands;
            public String legs;
            public String feet;
            public String mainhand;
            public String offhand;
            public String accessory1;
            public String accessory2;
        }

        public List<UserStats> listUsersStats = new List<UserStats>();
        public List<UserEquipment> listEquipment = new List<UserEquipment>();

        public String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void OnGet()
        {
            GetStatsSQL();
            GetEquipmentSQL();
        }

        public void GetStatsSQL()
        {
            String sql = "select * from [questboard_app].[dbo].[user_Stats]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user", HttpContext.Session.GetInt32("userID").ToString());
                    command.ExecuteNonQuery();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserStats userStats = new UserStats();

                            userStats.portrait = reader.GetString(1);
                            userStats.level = "" + reader.GetInt32(2);
                            userStats.class_specialty = reader.GetString(3);
                            userStats.hp = "" + reader.GetInt32(4);
                            userStats.power = "" + reader.GetInt32(5);
                            userStats.defense = "" + reader.GetInt32(6);
                            userStats.luck = "" + reader.GetInt32(7);

                            listUsersStats.Add(userStats);
                        }
                    }
                }
                sqlConn.Close();
            }
        }

        public void GetEquipmentSQL()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter user = new SqlParameter("@user", SqlDbType.VarChar, 50);
                user.Value = HttpContext.Session.GetInt32("userID").ToString();
                command.Parameters.Add(user);

                SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                optionName.Value = "sql_ViewEquipment";
                command.Parameters.Add(optionName);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserEquipment userEquipment = new UserEquipment();

                        userEquipment.head = reader.GetString(1);
                        userEquipment.shoulders = reader.GetString(2);
                        userEquipment.chest = reader.GetString(3);
                        userEquipment.hands = reader.GetString(4);
                        userEquipment.legs = reader.GetString(5);
                        userEquipment.feet = reader.GetString(6);
                        userEquipment.mainhand = reader.GetString(7);
                        userEquipment.offhand = reader.GetString(8);
                        userEquipment.accessory1 = reader.GetString(9);
                        userEquipment.accessory2 = reader.GetString(10);

                        listEquipment.Add(userEquipment);
                    }
                }
                connection.Close();
            }
        }
    }
}
