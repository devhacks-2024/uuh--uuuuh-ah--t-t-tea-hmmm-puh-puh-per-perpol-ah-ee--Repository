using Discord.WebSocket;
using Discord;
using Hackathon.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	private const int ITEMS_PER_SHOP_PAGE = 5;
	private const string SHOP_NAME = "**Magic store**";
	private const int ITEMS_PER_INFO_PAGE = 1;

	// Similear to show shop, however just for search term items and shows more data `shop_page_<searchterm>_#`
	public async Task ShowItemPage(ISocketMessageChannel location, String searchTerm, int pageIndex, List<Item> items, IUserMessage existingMessage = null)
	{
		if(searchTerm.Contains("_"))
		{
			await location.SendMessageAsync("Cannot have '_' in search term!");
			return;
		}

		int totalPages = (int)Math.Ceiling(items.Count / (double)ITEMS_PER_INFO_PAGE);

		EmbedBuilder window = new EmbedBuilder()
			.WithTitle($"Showing {items.Count} results for: {searchTerm}")
			.WithFooter(footer => footer.Text = $"Page {pageIndex + 1} of {totalPages}");

		// Add items to the window
		for(int i = pageIndex * ITEMS_PER_INFO_PAGE; i < Math.Min((pageIndex + 1) * ITEMS_PER_INFO_PAGE, items.Count); i++)
		{
			window.AddField($"{items[i].Name}: {items[i].GPCost}gp", items[i].Description + "\n---\n\n");
		}

		// nav buttons
		// Page switching logic is inside InteractionHandler......yes I know.
		// custom id is used to determine the logic for what is interacted with. We are using `shop_page_<searchterm>_#`, for a button and once pressed will show that item page
		MessageComponent component = new ComponentBuilder()
			.WithButton(emote: new Emoji("\u2B05"), customId: $"item_page_{searchTerm}_{pageIndex - 1}", disabled: pageIndex == 0)
			.WithButton(emote: new Emoji("\u27A1"), customId: $"item_page_{searchTerm}_{pageIndex + 1}", disabled: pageIndex == totalPages - 1)
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

	// a menu which has pages and shows item names
	public async Task ShowShopPage(ISocketMessageChannel location, int pageIndex, List<Item> items, IUserMessage existingMessage = null)
	{
		int totalPages = (int)Math.Ceiling(items.Count / (double)ITEMS_PER_SHOP_PAGE);

		EmbedBuilder window = new EmbedBuilder()
			.WithTitle(SHOP_NAME)
			.WithFooter(footer => footer.Text = $"Page {pageIndex + 1} of {totalPages}");

		// Add items to the window
		for(int i = pageIndex * ITEMS_PER_SHOP_PAGE; i < Math.Min((pageIndex + 1) * ITEMS_PER_SHOP_PAGE, items.Count); i++){
			window.AddField(items[i].Name, /*items[i].Description +*/ "\n---\n\n");
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

}
