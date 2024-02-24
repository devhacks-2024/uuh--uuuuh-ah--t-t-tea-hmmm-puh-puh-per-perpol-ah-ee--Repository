using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.DataObjects;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Modules;

[Group("player", "Commands assosiated with YOU!")]
public class PlayerModule : ModuleBase
{
	public PlayerModule(ILogger<ModuleBase> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction){}


	[SlashCommand("all", "Shows All Players")]
	public async Task DisplayAllCommand()
	{
		await DeferAsync();// stops error messages when there isnt an error

		List<PlayerObject> players = await _database.GetAllPlayers();
		foreach (PlayerObject player in players)
		{
			await Context.Channel.SendMessageAsync(player + "");
		}

		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."\
	}

	[SlashCommand("me", "WHo aRE YOU!")]
	public async Task InfoCommand()
	{
		// Get discord id and compare to database
		string discordId = Context.User.Id.ToString();
		var players = await _database.GetPlayer(discordId);

		if(players.Count == 0) await RespondAsync("You do not exist",ephemeral: true);

		// rare case where there is multiple results
		foreach(PlayerObject player in players)
		{
			await RespondAsync(player.player.name, ephemeral: true);
		}
	}


	// player inv

	// modify player, needing permissions?
}
