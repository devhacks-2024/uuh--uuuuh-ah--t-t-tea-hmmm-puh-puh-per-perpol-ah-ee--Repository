using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using Hackathon.Managers.Shop;
using Hackathon.DataObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using Discord;
using MongoDB.Bson.Serialization.Serializers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Hackathon.DataObjects.PlayerAdditions;

namespace Hackathon.Modules;

// Sub commands are an absolute mess

[DefaultMemberPermissions(GuildPermission.ManageChannels)] // This is a bad solution, however, it may be the best
[Group("admin", "Admin commands xolobot")]
public class AdminModule : ModuleBase
{
	public AdminModule(ILogger<ModuleBase> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction)
	{
	}



	[Group("add", "Add database entires")]
	public class AdminAddModule : ModuleBase
	{
		public AdminAddModule(ILogger<ModuleBase> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction)
		{
		}

		[SlashCommand("player", "Add player to database. Requires attachment with json")]
		public async Task AddPlayerCommand([Summary("jsonraw", "player json in .txt or .json. Supports lists for bulk adding")] IAttachment jsonInput)
		{
			// determine if list or single item.
			// add to the Database.AddPlayer function

			using(var web = new HttpClient())
			{
				var text = await web.GetStringAsync(jsonInput.Url);

				var json = JToken.Parse(text);

				if(json is JObject)
				{
					//single
					var player = JsonConvert.DeserializeObject<PlayerObject>(text);
					await _database.AddCharacter(player);
					await RespondAsync("Player added successfully.", ephemeral: true);
				}
				else if(json is JArray)
				{
					var players = JsonConvert.DeserializeObject<List<PlayerObject>>(text);
					await _database.AddCharacter(players);
					await RespondAsync($"{players.Count} players added successfully.", ephemeral: true);
				}
				else
				{
					//poopoo
					await RespondAsync("Invalid file.", ephemeral: true);
				}
			}

		}


		[SlashCommand("treasure", "Add gold,silver,copper to player")]
		public async Task AddPlayerTreasure([Summary("User", "target to give the treasure")] IUser target, [ComplexParameter]Treasure amount)
		{
			PlayerObject player = await _database.AddTreasure(target, amount);

			if(player != null)
			{
				await RespondAsync($"Added ({amount}) to {player.player.characterName}\nThere total is now: {player.treasure}");
			}
			else
			{
				await RespondAsync($"Invalid target");
			}
		}
	}

}
