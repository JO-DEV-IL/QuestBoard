using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Data;
using System.Data.SqlClient;

namespace QuestBoard.Pages.SqlHelpers
{
    public class sqlModel : PageModel
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
            public String head_img;

            public String shoulders;
            public String shoulders_img;

            public String chest;
            public String chest_img;

            public String hands;
            public String hands_img;

            public String legs;
            public String legs_img;

            public String feet;
            public String feet_img;

            public String mainhand;
            public String mainhand_img;

            public String offhand;
            public String offhand_img;

            public String accessory1;
            public String accessory1_img;

            public String accessory2;
            public String accessory2_img;
        }

        public class ShopItems
        {
            public String name;
            public String description;
            public String price;
            public String stock;
            public String image;
        }

        public List<ShopItems> listShopItems = new List<ShopItems>();
        public List<UserStats> listUsersStats = new List<UserStats>();
        public List<UserEquipment> listEquipment = new List<UserEquipment>();

        public String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void OnGet()
        {
        }

        public void GetStatsSQL(int user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter userID = new SqlParameter("@user", SqlDbType.Int);
                userID.Value = user;
                command.Parameters.Add(userID);

                SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                optionName.Value = "sql_ViewStats";
                command.Parameters.Add(optionName);

                connection.Open();

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
                connection.Close();
            }
        }

        public void GetEquipmentSQL(int user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter userID = new SqlParameter("@user", SqlDbType.Int);
                userID.Value = user;
                command.Parameters.Add(userID);

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
                        userEquipment.head_img = reader.GetString(2);

                        userEquipment.shoulders = reader.GetString(3);
                        userEquipment.shoulders_img = reader.GetString(4);

                        userEquipment.chest = reader.GetString(5);
                        userEquipment.chest_img = reader.GetString(6);

                        userEquipment.hands = reader.GetString(7);
                        userEquipment.hands_img = reader.GetString(8);

                        userEquipment.legs = reader.GetString(9);
                        userEquipment.legs_img = reader.GetString(10);

                        userEquipment.feet = reader.GetString(11);
                        userEquipment.feet_img = reader.GetString(12);

                        userEquipment.mainhand = reader.GetString(13);
                        userEquipment.mainhand_img = reader.GetString(14);

                        userEquipment.offhand = reader.GetString(15);
                        userEquipment.offhand_img = reader.GetString(16);

                        userEquipment.accessory1 = reader.GetString(17);
                        userEquipment.accessory1_img = reader.GetString(18);

                        userEquipment.accessory2 = reader.GetString(19);
                        userEquipment.accessory2_img = reader.GetString(20);

                        listEquipment.Add(userEquipment);
                    }
                }
                connection.Close();
            }
        }
        public void GetShopItemsSQL()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                optionName.Value = "sql_ItemShop";
                command.Parameters.Add(optionName);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ShopItems shopItems = new ShopItems();

                        shopItems.name = reader.GetString(0);
                        shopItems.description = reader.GetString(1);
                        shopItems.price = "" + reader.GetInt32(2);
                        shopItems.stock = "" + reader.GetInt32(3);
                        shopItems.image = reader.GetString(4);

                        listShopItems.Add(shopItems);
                    }
                }
                connection.Close();
            }
        }
    }
}
