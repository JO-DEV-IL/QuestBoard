using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestBoard;
using System.Data.SqlClient;

namespace QuestBoard.Pages
{
    public class ShopModel : PageModel
    {
        //public class ShopItems
        //{
        //    public String name;
        //    public String description;
        //    public String price;
        //    public String stock;
        //    public String image;
        //}

        //public List<ShopItems> listShopItems = new List<ShopItems>();

        public void OnGet()
        {
            //string user = HttpContext.Session.GetInt32("userID").ToString();

            //try
            //{
            //    String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            //    using (SqlConnection connection = new SqlConnection(connectionString))
            //    {
            //        connection.Open();

            //        String getItems = "select * from [questboard_app].[dbo].[test_Master_Shop_List]";

            //        using (SqlCommand command = new SqlCommand(getItems, connection))
            //        {
            //            //command.Parameters.AddWithValue("@user", user);

            //            using (SqlDataReader reader = command.ExecuteReader())
            //            {
            //                while (reader.Read())
            //                {
            //                    ShopItems shopItems = new ShopItems();

            //                    shopItems.name = reader.GetString(0);
            //                    shopItems.description = reader.GetString(1);
            //                    shopItems.price = "" + reader.GetInt32(2);
            //                    shopItems.stock = "" + reader.GetInt32(3);
            //                    shopItems.image = reader.GetString(4);

            //                    listShopItems.Add(shopItems);
            //                }
            //            }
            //        }
            //        connection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
        }
    }
}