using Discord.WebSocket;
using Discord;
using Hackathon.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;
using Hackathon.DataObjects.PlayerAdditions;

// Singleton
namespace Hackathon.Managers.Shop;
public class PlayerManager
{
	private static PlayerManager _instance;
	private PlayerManager() { }
	public static PlayerManager Instance
	{
		get{
			if(_instance == null){
				_instance = new PlayerManager();
			}

			return _instance;
		}
	}

    public EmbedBuilder CreatePlayerPage(ISocketMessageChannel location, PlayerObject player, SocketUser author, IUserMessage existingMessage = null)
	{
		/*EmbedBuilder window = new EmbedBuilder()
			.WithTitle(player.player.characterName);*/

		ulong discordId = ulong.Parse(player.player.discordId);

		//EmbedAuthorBuilder discordProfile = new EmbedAuthorBuilder()
		//.WithName(user.Nickname);
		//.WithIconUrl(user.AvatarId ?? user.GetDefaultAvatarUrl());


		EmbedBuilder playerDisplay = new EmbedBuilder()
			.WithAuthor(author)
			.WithTitle(player.player.characterName)
			.WithFooter($"Player: {player.player.playerName}");

		// First line: title & background
		playerDisplay.AddField(player.background.name, player.background.description);

		// Next line: Race, Class, & Subclass
		playerDisplay.AddField("Race:", player.race.name, true);

		if(player.classes.Count > 0)
		{
			String sClasses = "";
			String sSubclasses = "";
			for(int i = 0; i < player.classes.Count; i++)
			{
				if(i > 0) sClasses += "\r\n";
				if(i > 0 && sSubclasses != "") sSubclasses += "\r\n";

				sClasses += $"{player.classes[i].name}";
				sSubclasses += $"{player.classes[i].subtype}";
			}
			playerDisplay.AddField("Classes:", sClasses, true);
			playerDisplay.AddField("Sub-Class:", sSubclasses, true);
		}

		// Next line: Stats
		String sStr = player.ability_scores.str.ToString().PadLeft(3, ' ');
		String sDex = player.ability_scores.dex.ToString().PadLeft(3, ' ');
		String sCon = player.ability_scores.con.ToString().PadLeft(3, ' ');
		String sInt = player.ability_scores.intel.ToString().PadLeft(3, ' ');
		String sWis = player.ability_scores.wis.ToString().PadLeft(3, ' ');
		String sCha = player.ability_scores.cha.ToString().PadLeft(3, ' ');

		String sStats = String.Format("```STR:{0}\r\nDEX:{1}\r\nCON:{2}\r\nINT:{3}\r\nWIS:{4}\r\nCHA:{5}```",
						sStr, sDex, sCon, sInt, sWis, sCha);
		playerDisplay.AddField("Stats:", sStats);

		//playerDisplay.AddField("Class:", );

		// nav buttons
		// Page switching logic is inside InteractionHandler......yes I know.
		// custom id is used to determine the logic for what is interacted with. We are using shop_page_#, for a button and once pressed will show that shop page
		/*MessageComponent component = new ComponentBuilder()
			.WithButton("Inventory", customId: $"shop_page_", disabled: player.inventory.Count == 0)
			.WithButton("Other I forgot", customId: $"shop_page_", disabled: pageIndex == totalPages - 1)
			.Build();*/


		//RespondAsync(embed: playerDisplay.Build(), ephemeral: true);
		return playerDisplay;

	}

	public EmbedBuilder CreatePlayerInventroyPage(PlayerObject player, bool showAsList)
	{
		EmbedBuilder window = new EmbedBuilder()
			.WithTitle($"{player.player.characterName}'s Inventory");

		// showAsList = false, we display large navi like shop
		// Do some combinining items?

		if(showAsList)
		{
			foreach(Item item in player.inventory)
			{
				window.AddField($"{item.name}",$"{item.shortdescription}",true);
			}
		}
		else
		{
			// Show as large shoplike page
		}

		return window;
	}
}
