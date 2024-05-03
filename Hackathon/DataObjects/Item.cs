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
	public int cost { get; set; }
	public string name { get; set; }
	public string longdescription { get; set; }

	public string shortdescription { get; set; }
	public string lore { get; set; }
	public string weight { get; set; }//optional
	public string[] tags { get; set; }// optional

	public string imgUrl { get; set; }//optional

	public string TagsToString()
	{
		if(tags == null) return "";

		StringBuilder sb = new StringBuilder();
		foreach(string tag in tags)
		{
			sb.Append(tag).Append(", ");
		}

		sb.Remove(sb.Length - 2, 2);

		return sb.ToString();
	}

	public bool ContainsFilter(String filter)
	{
		if(name.Contains(filter, StringComparison.OrdinalIgnoreCase)) return true;

		foreach(string tag in tags)
		{
			if(tag.Contains(filter, StringComparison.OrdinalIgnoreCase)) return true;
		}
		return false;
	}

	public static List<Item> FilterItems(List<Item> list, string searchTerm)
	{
		if(string.IsNullOrEmpty(searchTerm))
		{
			return list;
		}

		return list.Where(item => item.ContainsFilter(searchTerm)).ToList();
	}
}
