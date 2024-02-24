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
		foreach (PlayerObject player in players)
		{
			await Context.Channel.SendMessageAsync(player + "");
		}

		await FollowupAsync("hmmmmmmmmmmmm");// stops the indefinate "* * * xolobot is thinking..."\
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
			.WithFooter(player.player.playerName);

		//playerDisplay.AddField("Stats:", "s");
		playerDisplay.AddField("Race:", player.race.name);

		if (player.classes.Count > 0)
		{
			String sClasses = "";
			for (int i = 0; i < player.classes.Count; i++)
			{
				sClasses += $"{i+1}: {player.classes[i].name} ";

				//playerDisplay.AddField(" ", $"{i}: {player.classes[i].name}", true);
			}
			playerDisplay.AddField("Classes:", sClasses);
		}
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
