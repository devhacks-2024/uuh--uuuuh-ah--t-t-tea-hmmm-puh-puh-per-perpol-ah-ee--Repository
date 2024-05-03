using Discord;
using Discord.WebSocket;
using Hackathon.DataObjects;
using Hackathon.Managers.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Managers;

internal class ItemManager
{
	private static ItemManager _instance;
	private ItemManager() { }
	public static ItemManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new ItemManager();
			}

			return _instance;
		}
	}

	// Single item, nothing about switching
	public EmbedBuilder CreateItemDisplay(Item item)
	{
		EmbedBuilder itemWindow = new EmbedBuilder()
			.WithTitle($"**{item.name}**\n**{item.cost}** ***gp***")
			.WithDescription($"> *{item.TagsToString()}*")
			//.WithFooter(footer => footer.Text = $"{items.Count} results for: '{searchTerm}'\nPage {pageIndex + 1} of {totalPages}")
			.WithImageUrl(item.imgUrl ?? "");// This makes it only possible for 1 item at a time

		itemWindow.AddField($"**Description**:", $"{item.longdescription}");

		return itemWindow;
	}

	public (EmbedBuilder,ComponentBuilder) CreateItemDisplayList(String contextCategory, String filterCategory, int pageIndex, List<Item> items)
	{
		// filterCategory is either the searchTerm for this list, or some other category like "inventory"
		// In this function it doesnt matter and is unkown what it is....this function doesnt also care, its just for displaying page count and transitioning
		// Multiple inventories at the same time may break things?!?!?! I have tested this and it doesnt at all. This is because InterationHandle references the component used
		int totalPages = items.Count;
		Item item = items[pageIndex];

		// Create man window
		EmbedBuilder window = CreateItemDisplay(item);

		// Nav Features
		window.WithFooter(footer => footer.Text = $"{items.Count} results for: '{filterCategory}'\nPage {pageIndex + 1} of {totalPages}");
		// Page switching logic is inside InteractionHandler......yes I know. 
		var component = new ComponentBuilder();
		if(items.Count > 1)
		{
			component
				.WithButton(emote: new Emoji("\u2B05"), customId: $"item-nav_{pageIndex - 1}_{contextCategory}_{filterCategory}", disabled: pageIndex == 0)
			.WithButton(emote: new Emoji("\u27A1"), customId: $"item-nav_{pageIndex + 1}_{contextCategory}_{filterCategory}", disabled: pageIndex == totalPages - 1);
		}

		return (window, component);
	}
}
