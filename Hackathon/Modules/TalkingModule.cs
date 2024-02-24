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


public class TalkingModule
{
	protected readonly ILogger<TalkingModule> _logger;
	protected readonly MongoDBService _database;
	protected readonly OpenAIService _openAI;
	protected readonly DiscordSocketClient _client;
	protected readonly InteractionHandler _interaction;

	public TalkingModule(ILogger<TalkingModule> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction)
	{
		_logger = logger;
		_database = mongoDbService;
		_openAI = openAIService;
		_client = client;
		_interaction = interaction;

		_interaction.OnPostBotMention += ScanForAIResponse;
		// maybe other events?
	}

	private async void ScanForAIResponse(Object sender, BotResponseArgs args)
	{
		string response = args.Response!.ToLower();
		// scan output from ai
		// able to change it in args
	}

}