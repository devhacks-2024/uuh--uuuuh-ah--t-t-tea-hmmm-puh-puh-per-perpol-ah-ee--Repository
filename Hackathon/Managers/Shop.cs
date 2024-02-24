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
public class Shop
{
	private static Shop _instance;
	private Shop() { }
	public static Shop Instance
	{
		get{
			if(_instance == null){
				_instance = new Shop();
			}

			return _instance;
		}
	}

	private const int ITEMS_PER_PAGE = 5;
	private const string SHOP_NAME = "**Magic store**";

	// a menu which has pages and shows item names
	public async Task ShowShopPage(ISocketMessageChannel location, int pageIndex, List<Item> items){
		int totalPages = (int)Math.Ceiling(items.Count / (double)ITEMS_PER_PAGE);

		EmbedBuilder window = new EmbedBuilder()
			.WithTitle(SHOP_NAME)
			.WithFooter(footer => footer.Text = $"Page {pageIndex + 1} of {totalPages}");

		// Add items to the window
		for(int i = pageIndex * ITEMS_PER_PAGE; i < Math.Min((pageIndex + 1) * ITEMS_PER_PAGE, items.Count); i++){
			window.AddField(items[i].Name, /*items[i].Description +*/ "\n---\n\n");
		}

		// nav buttons
		// Page switching logic is inside InteractionHandler......yes I know.
		MessageComponent component = new ComponentBuilder()
			.WithButton(emote: new Emoji("\u2B05"), customId: $"shop_page_{pageIndex-1}", disabled: pageIndex == 0)
			.WithButton(emote: new Emoji("\u27A1"), customId: $"shop_page_{pageIndex+1}", disabled: pageIndex == totalPages - 1)
			.Build();

		await location.SendMessageAsync(embed: window.Build(), components: component);
	}

}
