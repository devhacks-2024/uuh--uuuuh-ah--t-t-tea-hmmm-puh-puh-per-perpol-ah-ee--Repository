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

	private async void OpenShop(ISocketMessageChannel location)
	{
		var items = await _database.GetShopItems();
		await ShopManager.Instance.ShowShopPage(location, 0, items);
	}

	[SlashCommand("view", "View specifc items")]
	public async Task ItemInfoCommand(
		[Summary("query", "items with names and tags containing")]
		string searchTerm = "")
	{
		await DeferAsync();// stops error messages when there isnt an error
		if(searchTerm == null)
		{
			await RespondAsync("Invalid search term!", ephemeral: true);
			return;
		}
		else if(searchTerm.Contains("_"))
		{
			await RespondAsync("Cannot have '_' in search term!", ephemeral: true);
			return;
		}


		// This is a really bad function name. This string is guerenteed to NEVER be null, however...it may be empty
		// When empty, search ALL
		// Debatebly when searchTerm = "", it will already get everything, however, it may be better to be expld here.
		List<DataObjects.Item> items;
		if(string.IsNullOrEmpty(searchTerm))
		{
			items = await _database.GetShopItems();
			searchTerm = "all items";
		}
		else
		{
			items = await _database.GetShopItems(searchTerm);
		}

		if(items != null && items.Count > 0)
		{
			await ShopManager.Instance.ShowItemPage(Context.Channel, searchTerm, 0, items, Context.User);
		}
		else
		{
			await FollowupAsync("I dont have any items like that!");
			return;
		}

		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."
	}
}