using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Modules;

[Group("player", "Commands assosiated with YOU!")]
internal class PlayerModule : CommandModule
{
	public PlayerModule(ILogger<CommandModule> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction) : base(logger, mongoDbService, openAIService, client, interaction)
	{
	}
}
