using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LootBox.Api.Controllers
{
	[ApiController]
	[Route("")]
	public class HomeController : ControllerBase
	{
		[HttpGet]
		[Produces(MediaTypeNames.Text.Plain)]
		public string Get() => "LootBox API\nv0.0.1";
	}
}
