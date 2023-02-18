using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;

namespace QuestBoard.Pages
{
	public class HomeModel : PageModel
	{
		private readonly ILogger<HomeModel> _logger;

		public HomeModel(ILogger<HomeModel> logger)
		{
			_logger = logger;
		}
		public void OnGet()
		{
		}
	}
}