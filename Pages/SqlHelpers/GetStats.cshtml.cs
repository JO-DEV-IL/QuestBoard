using Microsoft.AspNetCore.Mvc;
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

        public void GetStatsSQL(string user)
        {
            SqlConnection sqlConn = new SqlConnection("Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseJO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            string sql = "select * from [questboard_app].[dbo].[user_Stats] where userID = @user";
            using (sqlConn)
            {
                sqlConn.Open();
                using (SqlCommand command = new SqlCommand(sql, sqlConn))
                {
                    command.Parameters.AddWithValue("@user", user);
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

        public void GetEquipmentSQL(string user)
        {
            SqlConnection sqlConn = new SqlConnection("Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseJO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            string sql =
                "SELECT"
                    + " u.userID,"
                    + "a.name as Head,"
                    + "b.name as Shoulders,"
                    + "c.name as Chest,"
                    + "d.name as Hands,"
                    + "e.name as Legs,"
                    + "f.name as Feet,"
                    + "g.name as Mainhand,"
                    + "h.name as Offhand,"
                    + "i.name as Accessory1,"
                    + "j.name as Accessory2"
                + " FROM [questboard_app].[dbo].[user_Equipment] u"
                    + " left join [questboard_app].[dbo].[master_Equipment] a on u.head = a.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] b on u.shoulders = b.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] c on u.chest = c.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] d on u.hands = d.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] e on u.legs = e.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] f on u.feet = f.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] g on u.mainhand = g.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] h on u.offhand = h.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] i on u.accessory1 = i.id"
                    + " left join [questboard_app].[dbo].[master_Equipment] j on u.accessory2 = j.id"
                + " WHERE u.userID = @user";
            using (sqlConn)
            {
                sqlConn.Open();
                using (SqlCommand command = new SqlCommand(sql, sqlConn))
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.ExecuteNonQuery();
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
                }
                sqlConn.Close();
            }
        }
    }
}
