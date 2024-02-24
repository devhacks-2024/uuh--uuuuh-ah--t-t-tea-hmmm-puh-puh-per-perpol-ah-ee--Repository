using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using Hackathon.Managers.Shop;

namespace Hackathon.Modules;

[Group("xolo", "commands for interacting with xolobot")]
public class ShopModule : CommandModule
{
	public ShopModule(ILogger<CommandModule> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction){}


	[SlashCommand("shop", "Opens Shop")]
	public async Task ShopCommand()
	{
		await DeferAsync();// stops error messages when there isnt an error
		OpenShop(Context.Channel);
		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."
	}

	private async void OpenShop(ISocketMessageChannel location)
	{
		var items = await _database.GetAllShopItems();
		await ShopManager.Instance.ShowShopPage(location, 0, items);
	}




}