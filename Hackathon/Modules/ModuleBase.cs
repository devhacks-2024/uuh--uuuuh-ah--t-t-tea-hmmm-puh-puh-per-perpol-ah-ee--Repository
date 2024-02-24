using Discord.Interactions;
using Discord.WebSocket;
using Hackathon.Services;
using Microsoft.Extensions.Logging;

namespace Hackathon.Modules;
public abstract class ModuleBase : InteractionModuleBase<SocketInteractionContext>
{
	protected readonly ILogger<ModuleBase> _logger;
	protected readonly MongoDBService _database;
	protected readonly OpenAIService _openAI;
	protected readonly DiscordSocketClient _client;
	protected readonly InteractionHandler _interaction;

	public ModuleBase(ILogger<ModuleBase> logger, MongoDBService mongoDbService, OpenAIService openAIService, DiscordSocketClient client, InteractionHandler interaction)
	{
		_logger = logger;
		_database = mongoDbService;
		_openAI = openAIService;
		_client = client;
	}

	/*[SlashCommand("test", "Just a test command")]
	public async Task TestCommand()
	{
		await RespondAsync("Hello There");
	}*/
}
