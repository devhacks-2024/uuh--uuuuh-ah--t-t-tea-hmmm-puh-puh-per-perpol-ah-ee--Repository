using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.Services;
using Microsoft.Extensions.Logging;

namespace Hackathon.Modules;

[Group("xolo", "commands for interacting with xolobot")]
public class ShopModule : CommandModule
{
	public ShopModule(ILogger<CommandModule> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction){}


	[SlashCommand("shop", "Opens Shop")]
	public async Task ShopCommand()
	{
		await RespondAsync("test 2");
	}


}