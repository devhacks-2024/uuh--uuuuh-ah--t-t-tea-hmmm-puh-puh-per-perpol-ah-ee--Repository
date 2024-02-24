using Hackathon.DataObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Services;

public class MongoDBSettings
{
	public string ConnectionString { get; set; }
	public string DatabaseName { get; set; }
}
public class MongoDBService
{
	private readonly IMongoDatabase _database;
	private readonly ILogger _logger;

	public MongoDBService(IOptions<MongoDBSettings> settings, ILogger<MongoDBService> logger)
	{
		var client = new MongoClient(settings.Value.ConnectionString);
		_database = client.GetDatabase(settings.Value.DatabaseName);

		_logger = logger;
		_logger.LogInformation($"Connected to MongoDB {_database.DatabaseNamespace}");
	}

	public async Task<List<Item>> GetShopItems()
	{
		try
		{
			var shopItemsCollection = _database.GetCollection<Item>("ShopItems");
			var items = await shopItemsCollection.Find(_ => true).ToListAsync();
			return items;
		}
		catch(Exception ex)
		{
			return new List<Item> { new Item() };
		}
	}

	public async Task<List<Item>> GetShopItems(string searchTerm)
	{
		var itemCollection = _database.GetCollection<Item>("ShopItems");

		var nameFilter = Builders<Item>.Filter.Regex("Name", new BsonRegularExpression(searchTerm, "i"));
		var tagsFilter = Builders<Item>.Filter.Regex("Tags", new BsonRegularExpression(searchTerm, "i"));

		var combinedFilter = Builders<Item>.Filter.Or(nameFilter, tagsFilter);

		List<Item> items = await itemCollection.Find(combinedFilter).ToListAsync();

		return items;
	}

	public async Task<List<PlayerObject>> GetAllPlayers()
	{
		var playerCollection = _database.GetCollection<PlayerObject>("Players");
		List<PlayerObject> players = await playerCollection.Find(_ => true).ToListAsync();
		return players;
	}

	public async Task<List<PlayerObject>> GetPlayer(string discordId)
	{
		var playerCollection = _database.GetCollection<PlayerObject>("Players");
		var filter = Builders<PlayerObject>.Filter.Eq("player.discordId", discordId);
		
		List<PlayerObject> players = await playerCollection.Find(filter).ToListAsync();

		if(players.Count > 1) Console.WriteLine("huh, it seems discord id:'" + discordId + "' has " + players.Count + "players assosiated with them.");

		return players;
	}

}
