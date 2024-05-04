using Azure;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.DataObjects;
using Hackathon.DataObjects.PlayerAdditions;
using Hackathon.Managers;
using Hackathon.Managers.Shop;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
		Console.WriteLine(players.Count);

		ShowPlayerEmbed(Context.Channel, players[0]);
		/*foreach (PlayerObject player in players)
		{
			ShowPlayerEmbed(Context.Channel, player);
			//await Context.Channel.SendMessageAsync(player + "");
		}*/

		//await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."\
	}

	[SlashCommand("me", "WHo aRE YOU!")]
	public async Task InfoCommand()
	{
		// Get discord id and compare to database
		string discordId = Context.User.Id.ToString();
		var players = await _database.GetPlayer(discordId);

        if (players.Count == 0) await RespondAsync("You do not exist",ephemeral: true);

		// rare case where there is multiple results
		foreach(PlayerObject player in players)
		{
			//await RespondAsync(player.player.characterName, ephemeral: true);
			ShowPlayerEmbed(Context.Channel, player);
		}
	}

	private void ShowPlayerEmbed(ISocketMessageChannel location, PlayerObject player)
	{
		ulong discordId = ulong.Parse(player.player.discordId);

		EmbedBuilder page = PlayerManager.Instance.CreatePlayerPage(location,player, Context.Guild.GetUser(discordId));

		RespondAsync(embed: page.Build(), ephemeral: true);
	}

	[SlashCommand("inventory", "Shows your inventory")]
	public async Task ShowInventoryCommand(
		[Summary("Filter", "Only display items using this filter (name and tags.)")]
		string filter = "",
		[Summary("list", "show all items as a single list.")]
		bool showAsList = false)
	{
		await DeferAsync();// stops error messages when there isnt an error

		if(filter == null)
		{
			await RespondAsync("Invalid search term!", ephemeral: true);
			return;
		}
		else if(filter.Contains("_"))
		{
			await RespondAsync("Cannot have '_' in search term!", ephemeral: true);
			return;
		}

		ulong discordId = Context.User.Id;

		var user =  await _database.GetPlayer(discordId.ToString());

		List<DataObjects.Item> items;
		if(string.IsNullOrEmpty(filter))
		{
			items = user.First().inventory;
			filter = "all items";
		}
		else
		{
			// filter containing name, or COMPLETE tag
			items = Item.FilterItems(user.First().inventory, filter);
		}

		if(items != null && items.Count > 0)
		{
			if(showAsList)
			{
				// TODO: add filter
				EmbedBuilder page = PlayerManager.Instance.ListPlayerInventory(user.First());
				await RespondAsync(embed: page.Build(), ephemeral: true); // TODO: DETERMINE if should use EPHEMERAL 
			}
			else
			{
				await PlayerManager.Instance.ShowInventoryPage(Context.Channel,0,items, filter, discordId.ToString());
			}
		}
		else
		{
			// empty inventory
			await FollowupAsync("There is nothing in your inventory like that!");
			return;
		}


		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."
	}


	// player inv

	// modify player, needing permissions?
}
