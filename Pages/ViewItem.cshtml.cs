using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestBoard.Pages.Users;
using System.Data;
using System.Data.SqlClient;

namespace QuestBoard.Pages
{
    public class ViewItemModel : PageModel
    {
        public String successMessage = "";
        public String errorMessage = "";

        public String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public class UserItems
        {
            public String name;
            public String quantity;
            public String rarity;
            public String description;
            public String image;
            public bool equipable;
        }
        public List<UserItems> listUsersItems = new List<UserItems>();

        public void OnGet()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter itemName = new SqlParameter("@itemName", SqlDbType.VarChar, 50);
                itemName.Value = HttpContext.Request.Query["item"].ToString();
                command.Parameters.Add(itemName);

                SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                optionName.Value = "sql_ViewItem";
                command.Parameters.Add(optionName);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserItems userItems = new UserItems();

                        userItems.name = reader.GetString(0);
                        userItems.description = reader.GetString(1);
                        userItems.rarity = reader.GetString(2);
                        userItems.image = reader.GetString(3);
                        userItems.quantity = reader.GetInt32(4).ToString();
                        userItems.equipable = reader.GetBoolean(5);

                        listUsersItems.Add(userItems);
                    }
                }
                connection.Close();
            }
        }

        public void OnPost()
        {
            if (Request.Query["handler"].ToString() == "open")
            {
                //OpenChest();
            }
            else if (Request.Query["handler"].ToString() == "equip")
            {
                Equip();
                Response.Redirect("/Users/Inventory");
            }
            else if (Request.Query["handler"].ToString() == "unequip")
            {
                Unequip();
                Response.Redirect("/Users/Inventory");
            }
        }
        public void Equip()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter itemName = new SqlParameter("@itemName", SqlDbType.VarChar, 50);
                itemName.Value = HttpContext.Session.GetString("SelectedItem");
                command.Parameters.Add(itemName);

                SqlParameter user = new SqlParameter("@user", SqlDbType.Int);
                user.Value = HttpContext.Session.GetInt32("userID");
                command.Parameters.Add(user);

                SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                optionName.Value = "sql_Equip";
                command.Parameters.Add(optionName);

                connection.Open();
                connection.Close();
            }
        }
        public void Unequip()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("[questboard_app].[dbo].[qb_master_Proc]", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter itemName = new SqlParameter("@itemName", SqlDbType.VarChar, 50);
                itemName.Value = HttpContext.Session.GetString("SelectedItem");
                command.Parameters.Add(itemName);

                SqlParameter user = new SqlParameter("@user", SqlDbType.Int);
                user.Value = HttpContext.Session.GetInt32("userID");
                command.Parameters.Add(user);

                SqlParameter optionName = new SqlParameter("@Option", SqlDbType.VarChar, 50);
                optionName.Value = "sql_Unequip";
                command.Parameters.Add(optionName);

                connection.Open();
                connection.Close();
            }
        }
    }
}
