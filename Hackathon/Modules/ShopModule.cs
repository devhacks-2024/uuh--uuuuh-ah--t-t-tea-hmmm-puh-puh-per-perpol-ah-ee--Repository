using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using Hackathon.Managers.Shop;
using Hackathon.DataObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;

namespace Hackathon.Modules;

[Group("shop", "commands for interacting with xolobot")]
public class ShopModule : ModuleBase
{
	public ShopModule(ILogger<ModuleBase> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction){}


	[SlashCommand("open", "Opens Shop")]
	public async Task ShopCommand()
	{
		await DeferAsync();// stops error messages when there isnt an error
		OpenShop(Context.Channel);
		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."
	}

	[SlashCommand("view", "View specifc items")]
	public async Task ItemInfoCommand(
		[Summary("query", "items with names and tags containing")]
		string searchTerm)
	{
		await DeferAsync();// stops error messages when there isnt an error
		OpenItemPage(Context.Channel, searchTerm);
		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."
	}

	private async void OpenShop(ISocketMessageChannel location)
	{
		var items = await _database.GetShopItems();
		await ShopManager.Instance.ShowShopPage(location, 0, items);
	}

	private async void OpenItemPage(ISocketMessageChannel location, String searchTerm)
	{
		var items = await _database.GetShopItems(searchTerm);
		await ShopManager.Instance.ShowItemPage(location, searchTerm, 0, items, Context.User);
	}

	// Buy item
	// Buy is on item view page


	// etc.




}