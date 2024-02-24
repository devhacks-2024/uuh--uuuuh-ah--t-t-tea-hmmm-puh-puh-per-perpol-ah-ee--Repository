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
public class PlayerModule : CommandModule
{
	public PlayerModule(ILogger<CommandModule> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction)
	{
	}


	[SlashCommand("all", "Shows All Players")]
	public async Task ShopCommand()
	{
		await DeferAsync();// stops error messages when there isnt an error

		List<PlayerObject> players = await _database.GetAllPlayers();
		foreach (PlayerObject player in players)
		{
			await Context.Channel.SendMessageAsync(player + "");
		}

		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."\
	}

	// player inv

	// modify player, needing permissions?
}
