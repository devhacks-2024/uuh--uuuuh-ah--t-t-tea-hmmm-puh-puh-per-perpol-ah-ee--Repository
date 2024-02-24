using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

}
