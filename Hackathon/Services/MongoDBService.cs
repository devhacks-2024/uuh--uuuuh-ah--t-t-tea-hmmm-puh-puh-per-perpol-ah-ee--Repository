using Hackathon.DataObjects;
using Hackathon.DataObjects.PlayerAdditions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

		var nameFilter = Builders<Item>.Filter.Regex("name", new BsonRegularExpression(searchTerm, "i"));
		var tagsFilter = Builders<Item>.Filter.Regex("tags", new BsonRegularExpression(searchTerm, "i"));

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

		if(players.Count > 1) _logger.LogInformation("huh, it seems discord id:'" + discordId + "' has " + players.Count + "players assosiated with them.");

		return players;
	}


	public async Task<int> BuyItem(string discordId, string itemName)
	{
		var itemCollection = _database.GetCollection<Item>("ShopItems");
		var item = await itemCollection.Find(i => i.name == itemName).FirstOrDefaultAsync();
		
		var playerCollection = _database.GetCollection<PlayerObject>("Players");
		var filter = Builders<PlayerObject>.Filter.Eq("player.discordId", discordId);
		var player = await playerCollection.Find(filter).FirstOrDefaultAsync(); // This breaks if there are multiple characters assosiated with a player.

		if(item == null || player == null)
		{
			// item not found
			return -1;
		}

		if(player.treasure.gold < item.cost)
		{
			//poor
			return 0;
		}

		// Update gold and inv
		var updatePlayer = Builders<PlayerObject>.Update
			.Push("inventory", item)
			.Set(p => p.treasure.gold, player.treasure.gold - item.cost);

		await playerCollection.UpdateOneAsync(filter, updatePlayer);

		// Update shop
		var deleteFilter = Builders<Item>.Filter.Eq("name", itemName);
		await itemCollection.DeleteOneAsync(deleteFilter);

		return 1;
	}

	public async Task AddCharacter(PlayerObject character)
	{
		try
		{
			var collection = _database.GetCollection<PlayerObject>("Players");

			// Check if the character already exists based on DiscordId
			var existingPlayer = await collection.Find(p => p.player.discordId == character.player.discordId).FirstOrDefaultAsync();
			if(existingPlayer != null)
			{
				_logger.LogInformation($"Character already exists for Discord ID: {character.player.discordId}. No new character added.");
				return; // Exit the method to avoid adding a duplicate
			}

			await collection.InsertOneAsync(character);
			_logger.LogInformation($"Added new character. <@{character.player.discordId}> as {character.player.characterName}");
		}
		catch(Exception ex)
		{
			_logger.LogError($"Error adding character: {ex.Message}");
			throw;
		}
	}


	public async Task AddCharacter(List<PlayerObject> characters)
	{
		try
		{
			var collection = _database.GetCollection<PlayerObject>("Players");

			// Find existing characters to prevent duplicates
			var discordIds = characters.Select(c => c.player.discordId).ToList();
			var existingPlayers = await collection.Find(p => discordIds.Contains(p.player.discordId)).ToListAsync();
			var existingPlayerIds = new HashSet<string>(existingPlayers.Select(p => p.player.discordId));

			// Filter out characters that already exist
			var newCharacters = characters.Where(c => !existingPlayerIds.Contains(c.player.discordId)).ToList();
			if(!newCharacters.Any())
			{
				_logger.LogInformation("No new characters to add. All provided characters already exist in the database.");
				return;
			}

			await collection.InsertManyAsync(newCharacters);
			_logger.LogInformation($"Added {newCharacters.Count} new characters to the database.");
		}
		catch(MongoBulkWriteException ex)
		{
			_logger.LogError($"Bulk insert error: {ex.Message}");
			throw;
		}
		catch(Exception ex)
		{
			_logger.LogError($"Error adding characters: {ex.Message}");
			throw;
		}
	}

}
