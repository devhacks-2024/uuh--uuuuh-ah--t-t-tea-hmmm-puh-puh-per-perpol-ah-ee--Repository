using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon.DataObjects.PlayerAdditions;

namespace Hackathon.DataObjects;
public class PlayerObject
{
	public ObjectId _id { get; set; }
	public Player player { get; set; }
	public Race race { get; set; }
	public List<Class> classes { get; set; }
	public Background background { get; set; }
	public Details details { get; set; }
	public string alignment { get; set; }
	public AbilityScores ability_scores { get; set; }
	public List<string> languages { get; set; }
	public Treasure treasure { get; set; }
	//public List<Spell> spells { get; set; }
	public List<Item> inventory { get; set; }
}
