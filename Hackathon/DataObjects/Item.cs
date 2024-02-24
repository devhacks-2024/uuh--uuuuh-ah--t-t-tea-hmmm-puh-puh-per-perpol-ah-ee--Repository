using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.DataObjects;

// technically everything may be optional
// json has No rules, no boundaries, it doesnt flitch at tortue, human trafficking, or genocide. Its not loyal to a flag or country or any set of ideals
public class Item
{
	public ObjectId _id { get; set; }
	public double GPCost { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Lore { get; set; }
	public string Weight { get; set; }//optional
	public string[] Tags { get; set; }// optional
}
