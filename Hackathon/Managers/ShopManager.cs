﻿using Discord.WebSocket;
using Discord;
using Hackathon.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon.Services;

/*
- Difference between showShop and showItem.
	- showShop is browsing entire list, with no option to buy and small description. 
	- showItem does single item at a time nav list, with large description and buy option.
		- showItem is a list, because it can support search terms, however, generally its for 1 item.
 */

// Singleton
namespace Hackathon.Managers.Shop;
public class ShopManager
{
	private static ShopManager _instance;
	private ShopManager() { }
	public static ShopManager Instance
	{
		get{
			if(_instance == null){
				_instance = new ShopManager();
			}
			return _instance;
		}
	}

	private const int ITEMS_PER_SHOP_PAGE = 3;
	private const string SHOP_NAME = "**Magic store**";
	private readonly (float, float) SELL_PRICE_INC = (1.1f, 1.4f);
	private readonly (float, float) BUY_PRICE_DEC = (0.75f, 0.9f);
	private readonly Random random = new Random();


	// possible expansion for controlling how this function works (hagle)
	public float GetSellIncMultiplier()
	{
		// scale * range + base(min)
		double value = random.NextDouble() * (SELL_PRICE_INC.Item2 - SELL_PRICE_INC.Item1) + SELL_PRICE_INC.Item1;
		return (float)Math.Round(value, 3);
	}

	public float GetBuyDecMultiplier()
	{
		// scale * range + base(min)
		double value = random.NextDouble() * (BUY_PRICE_DEC.Item2 - BUY_PRICE_DEC.Item1) + BUY_PRICE_DEC.Item1;
		return (float)Math.Round(value, 3);
	}

	// Similear to show shop, however just for search term items and shows more data `shop_page_<searchterm>_#`
	// ONLY SHOWS 1 Item per page
	// List is already filtered and valid, search-term is just for display purposes.
	public async Task ShowItemPage(ISocketMessageChannel location, String searchTerm, int pageIndex, List<Item> items, IUser userInteractor, IUserMessage existingMessage = null)
	{
		int totalPages = items.Count;
		Item item = items[pageIndex];

		(EmbedBuilder, ComponentBuilder) builders = ItemManager.Instance.CreateItemDisplayList("SHOP",searchTerm,pageIndex,items);

		// Shop buy. This doesnt care about anything but whoever CREATED the page and the current item on display.
		// Shops must be ephemral??? unless we can buy for whoever clicked...however....
		// TODO: remove/move this to shop this
		builders.Item2.AddRow(new ActionRowBuilder().WithButton("***BUY NOW!***", customId: $"shop_buy_{item._id}_{userInteractor.Id.ToString()}"));


		// Edit existing shop menu, so it doesnt spam.
		if(existingMessage != null)
		{
			await existingMessage.ModifyAsync(msg => {
				msg.Embed = builders.Item1.Build();
				msg.Components = builders.Item2.Build();
			});
		}
		else
		{
			await location.SendMessageAsync(embed: builders.Item1.Build(), components: builders.Item2.Build());
		}
	}

	// a menu which has pages and shows item names
	public async Task ShowShopPage(ISocketMessageChannel location, int pageIndex, List<Item> items, IUserMessage existingMessage = null)
	{
		int totalPages = (int)Math.Ceiling(items.Count / (double)ITEMS_PER_SHOP_PAGE);

		EmbedBuilder window = new EmbedBuilder()
			.WithTitle(SHOP_NAME)
			.WithFooter(footer => footer.Text = $"Page {pageIndex + 1} of {totalPages}");

		// Add items to the window
		for(int i = pageIndex * ITEMS_PER_SHOP_PAGE; i < Math.Min((pageIndex + 1) * ITEMS_PER_SHOP_PAGE, items.Count); i++) {
			window.AddField($"\n**{items[i].name}**: ***{items[i].cost}***gp", $"> *{items[i].TagsToString()}*\n\n{items[i].shortdescription} \n\n──────────\n\n");
		}

		// nav buttons
		// Page switching logic is inside InteractionHandler......yes I know.
		// custom id is used to determine the logic for what is interacted with. We are using shop_page_#, for a button and once pressed will show that shop page
		MessageComponent component = new ComponentBuilder()
			.WithButton(emote: new Emoji("\u2B05"), customId: $"shop_page_{pageIndex-1}", disabled: pageIndex == 0)
			.WithButton(emote: new Emoji("\u27A1"), customId: $"shop_page_{pageIndex+1}", disabled: pageIndex == totalPages - 1)
			.Build();


		// Edit existing shop menu, so it doesnt spam.
		if(existingMessage != null)
		{
			await existingMessage.ModifyAsync(msg => {
				msg.Embed = window.Build();
				msg.Components = component;
			});
		}
		else
		{
			await location.SendMessageAsync(embed: window.Build(), components: component);
		}
	}

	public async void BuyItem(SocketMessageComponent caller, string itemId, MongoDBService databaseReference)
	{
		int result = await databaseReference.BuyItem(caller.User.Id.ToString(), itemId);

		if(result == -1)
		{
			await caller.RespondAsync("item or player is null in database", ephemeral: true);
		}
		else if(result == 0)// This should never get called
		{
			await caller.RespondAsync("You are too poor ;(", ephemeral: true);
		}
		else if(result == 1)
		{
			await caller.RespondAsync($"<@{caller.User.Id}>, is now the owner of: {itemId}");
		}
	}

	public async void SellItem(SocketMessageComponent caller, string playerId, string itemId, MongoDBService databaseReference)
	{
		//TODO: MAKE SURE CALLER ID IS PLAYER ID. this disallows other people from selling your items.
		int result = await databaseReference.SellItem(playerId, itemId);

		/*if(result == -1)
		{
		// ERROR WITH DATABASE
			//await caller.RespondAsync("item or player is null in database", ephemeral: true);
		}*/
		if(result == 0)// Item not in inventory
		{
			await caller.RespondAsync("The item is not in your inventory.", ephemeral: true);
		}
		else if(result == 1)
		{
			await caller.RespondAsync($"You have sold your item");
		}
	}
}
