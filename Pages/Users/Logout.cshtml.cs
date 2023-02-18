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
				String connectionString = "Data Source=LAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					String update = "UPDATE [questboard_app].[dbo].[users] SET is_loggedin = 0 WHERE username = @username";

					using (SqlCommand updateUser = new SqlCommand(update, connection))
					{
						if(HttpContext.Session.GetString("userActive") != null)
						{
							updateUser.Parameters.AddWithValue("@username", HttpContext.Session.GetString("userActive"));
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
