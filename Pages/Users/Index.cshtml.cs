using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QuestBoard.Pages.Users
{
	public class IndexModel : PageModel
	{
		// Global variable
		// Lists users via public class UserInfo params
		public List<UserInfo> listUsers = new List<UserInfo>();

		// Http Get method
		// On page load events, like Main()
		public void OnGet()
		{
			try
			{
				// Variable of SQL databse using connection string from db's properties
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

				// Try connection
				// If successful, open it and run sql command
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT userID,username,firstName,lastName,age,class,level,role,password FROM [questboard_app].[dbo].[users]";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								// reader.GetWhatever(column#)
								// reads SQL table and displays that info
								UserInfo userInfo = new UserInfo();
								userInfo.userID = "" + reader.GetInt32(0);
								userInfo.username = reader.GetString(1);
								userInfo.firstName = reader.GetString(2);
								userInfo.lastName = reader.GetString(3);
								userInfo.age = "" + reader.GetInt32(4);
								userInfo.class_specialty = reader.GetString(5);
								userInfo.level = "" + reader.GetInt32(6);
								userInfo.role = reader.GetString(7);
								userInfo.password = reader.GetString(8);

								// Add each row to the user list
								listUsers.Add(userInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Console log errors for testing
				Console.WriteLine(ex);
			}
			finally
			{
				// Close the SQL connection for security
				String connectionString = "Data Source=JO-DEV-IL;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=FalseLAPTOP-14G24561\\LOCALHOST;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Close();
				}
			}
		}
	}

	// Class composed of variable references
	// Accessed via new UserInfo()
	public class UserInfo
	{
		public String userID;
		public String username;
		public String firstName;
		public String lastName;
		public String age;
		public String class_specialty;
		public String level;
		public String role;
		public String password;
	}
}