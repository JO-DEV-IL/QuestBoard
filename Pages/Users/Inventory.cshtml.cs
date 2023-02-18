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
			public String image_path;
		}

		public List<UserItems> listUsersItems = new List<UserItems>();

		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=LAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String getItems = "select i.itemName, i.itemQuantity, i.description, i.image_path from [questboard_app].[dbo].[user_inventory] i join [questboard_app].[dbo].[users] u on u.userID = i.userID where i.userID = @userID order by itemName";

					using (SqlCommand command = new SqlCommand(getItems, connection))
					{
						command.Parameters.AddWithValue("@userID", HttpContext.Session.GetInt32("userID"));
						command.ExecuteNonQuery();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								UserItems userItems = new UserItems();
								userItems.name = reader.GetString(0);
								userItems.quantity = "" + reader.GetInt32(1);
								userItems.image_path = reader.GetString(3);

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