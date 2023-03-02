using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Data;

namespace QuestBoard.Pages.Users
{
	public class LogoutModel : PageModel
	{
		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String update = "UPDATE [questboard_app].[dbo].[master_Users] SET is_active = 0 WHERE id = @user";

					using (SqlCommand updateUser = new SqlCommand(update, connection))
					{
						if(HttpContext.Session.GetString("userActive") != null)
						{
							updateUser.Parameters.AddWithValue("@user", HttpContext.Session.GetInt32("userID").ToString());
							updateUser.ExecuteNonQuery();
						}
					}
					connection.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				HttpContext.Session.Clear();
				Response.Redirect("/Users/Login");
			}
		}
	}
}
