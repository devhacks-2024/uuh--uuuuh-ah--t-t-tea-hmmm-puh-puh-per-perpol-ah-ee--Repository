using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.DataObjects;
public class Item
{
	public ObjectId _id { get; set; }
	public double GPCost { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Lore { get; set; }
	public string Weight { get; set; }
	public string[] Tags { get; set; }
}
