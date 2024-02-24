using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.DataObjects;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
		var user = Context.Guild.GetUser(discordId);

		//EmbedAuthorBuilder discordProfile = new EmbedAuthorBuilder()
			//.WithName(user.Nickname);
		//.WithIconUrl(user.AvatarId ?? user.GetDefaultAvatarUrl());


		EmbedBuilder playerDisplay = new EmbedBuilder()
			.WithAuthor(Context.User)
			.WithTitle(player.player.characterName)
			.WithFooter($"Player: {player.player.playerName}");

		// First line: title & background
        playerDisplay.AddField(player.background.name, player.background.description);

		// Next line: Race, Class, & Subclass
        playerDisplay.AddField("Race:", player.race.name, true);

		if (player.classes.Count > 0)
		{
			String sClasses = "";
			String sSubclasses = "";
			for (int i = 0; i < player.classes.Count; i++)
			{
				if (i > 0) sClasses += "\r\n";
                if (i > 0 && sSubclasses != "") sSubclasses += "\r\n";

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


        RespondAsync(embed:playerDisplay.Build(), ephemeral: true);

	}


	// player inv

	// modify player, needing permissions?
}
