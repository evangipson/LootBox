using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

using LootBox.Domain.Models;
using LootBox.Logic.Managers;

namespace LootBox.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LootController(ILootManager lootManager) : ControllerBase
	{
		[HttpGet]
		[Produces(MediaTypeNames.Application.Json)]
		public Item Get() => lootManager.GetLoot();

		[HttpGet("[action]")]
		[Produces(MediaTypeNames.Image.Png)]
		public FileContentResult Image() => File(lootManager.GenerateLootImage(), MediaTypeNames.Image.Png);
	}
}
