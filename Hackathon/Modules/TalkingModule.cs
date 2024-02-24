using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.DataObjects;
using Hackathon.Modules;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hackathon.Services.InteractionHandler;

namespace Hackathon.Modules;
public class TalkingModule : ModuleBase
{
	public TalkingModule(ILogger<ModuleBase> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction)
	{
		interaction.OnPostBotMention += ScanAIResponse;
	}

	private async void ScanAIResponse(Object sender, BotResponseArgs args)
	{
		string response = args.Response!.ToLower();
        // scan output from ai
        // able to change it in args
        //await Console.Out.WriteLineAsync("YOOO");
    }

}