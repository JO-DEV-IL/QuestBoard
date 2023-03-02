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

		public class UserItems
		{
			public String name;
			public String quantity;
			public String rarity;
			public String description;
			public String image;
		}
		public List<UserItems> listUsersItems = new List<UserItems>();

		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String getItems =
						"SELECT M.name, M.description, M.rarity, M.image, I.quantity"
						+ " FROM [questboard_app].[dbo].[user_Inventory] I"
						+ " INNER JOIN [questboard_app].[dbo].[master_Misc_Items] M ON I.itemID = M.id"
						+ " WHERE M.name = @itemName"
						+ " UNION ALL"
						+ " SELECT M.name, M.description, M.rarity, M.image, I.quantity"
						+ " FROM [questboard_app].[dbo].[user_Inventory] I"
						+ " INNER JOIN [questboard_app].[dbo].[master_Equipment] M ON I.itemID = M.id"
						+ " WHERE M.name = @itemName";

					using (SqlCommand command = new SqlCommand(getItems, connection))
					{
						command.Parameters.AddWithValue("@itemName", HttpContext.Request.Query["item"].ToString());
						
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

		public void OnPost()
		{
			if (Request.Query["handler"].ToString() == "open")
			{
				OpenChest();
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

		public void OpenChest()
		{
			try
			{
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					SqlCommand command = new SqlCommand("[questboard_app].[dbo].[open_chest]", connection);
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@userID", HttpContext.Session.GetInt32("userID"));

					// Add output parameters
					SqlParameter lootName = new SqlParameter("@lootName", SqlDbType.VarChar, 20);
					lootName.Direction = ParameterDirection.Output;
					command.Parameters.Add(lootName);

					SqlParameter quantity = new SqlParameter("@quantity", SqlDbType.Int);
					quantity.Direction = ParameterDirection.Output;
					command.Parameters.Add(quantity);

					command.ExecuteNonQuery();

					// Get the values of the output parameters after executing the stored procedure
					string obtainedLootName = (string)lootName.Value;
					int obtainedQuantity = (int)quantity.Value;

					successMessage = "Chest was opened successfully! " + obtainedLootName + " x" + obtainedQuantity + " have been added to your inventory.";

					connection.Close();
				}
			}
			catch (Exception ex)
			{
				errorMessage = "Chest did not open!";
				Console.WriteLine(ex);
			}
		}
		public void Equip()
		{
			try
			{
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					SqlCommand command = new SqlCommand("[questboard_app].[dbo].[equip_item]", connection);
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@userID", HttpContext.Session.GetInt32("userID"));
					command.Parameters.AddWithValue("@item", HttpContext.Session.GetString("SelectedItem"));

					command.ExecuteNonQuery();

					connection.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public void Unequip()
		{
			try
			{
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					SqlCommand command = new SqlCommand("[questboard_app].[dbo].[unequip_item]", connection);
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@userID", HttpContext.Session.GetInt32("userID"));
					command.Parameters.AddWithValue("@item", HttpContext.Session.GetString("SelectedItem"));

					command.ExecuteNonQuery();

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
