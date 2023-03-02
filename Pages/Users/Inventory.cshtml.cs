using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestBoard.Pages.Users;
using System.Data.SqlClient;

namespace QuestBoard.Pages
{
    public class InventoryModel : PageModel
    {
        public class UserItems
        {
            public String name;
            public String quantity;
            public String rarity;
            public String image;
        }

        public List<UserItems> listUsersItems = new List<UserItems>();

        public void OnGet()
        {
            string user = HttpContext.Session.GetInt32("userID").ToString();

            try
            {
                String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String getItems =
                        "DECLARE @sql nvarchar(max);"
                        + "DECLARE @user int = @userID;"
                        + "DECLARE @tableName varchar(max) = @table;"
                        + " SET @sql = 'SELECT M.name, I.quantity, M.rarity, M.image FROM user_Inventory I LEFT JOIN ' + QUOTENAME(@tableName) + ' M ON I.itemID = M.id WHERE userID = @user';"
                        + " EXEC sp_executesql @sql";
                    
                    using (SqlCommand command = new SqlCommand(getItems, connection))
                    {
                        Console.WriteLine(user);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@userID", user);
                        command.Parameters.AddWithValue("@table", "master_Misc_Items");
                        command.ExecuteNonQuery();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserItems userItems = new UserItems();

                                userItems.name = reader.GetString(0);
                                userItems.quantity = "" + reader.GetInt32(1);
                                userItems.rarity = reader.GetString(2);
                                userItems.image = reader.GetString(3);

                                listUsersItems.Add(userItems);
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