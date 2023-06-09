using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestBoard.Pages.Users;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace QuestBoard.Pages
{
    public class InventoryModel : PageModel
    {
        public class UserItems
        {
            public String name;
            public String quantity;
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
                        "SELECT I.itemID, M.name, M.image, I.quantity"
                        + " FROM [questboard_app].[dbo].[user_Inventory] I"
                        + " INNER JOIN [questboard_app].[dbo].[master_Misc_Items] M ON I.itemID = M.id"
                        + " WHERE I.userID = @user"
                        + " UNION ALL"
                        + " SELECT I.itemId, M.name, M.image, I.quantity"
                        + " FROM [questboard_app].[dbo].[user_Inventory] I"
                        + " INNER JOIN [questboard_app].[dbo].[master_Equipment] M ON I.itemID = M.id"
                        + " WHERE I.userID = @user";


                    using (SqlCommand command = new SqlCommand(getItems, connection))
                    {
                        command.Parameters.AddWithValue("@user", user);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserItems userItems = new UserItems();

                                userItems.name = reader.GetString(1);
                                userItems.image = reader.GetString(2);
                                userItems.quantity = reader.GetInt32(3).ToString();

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