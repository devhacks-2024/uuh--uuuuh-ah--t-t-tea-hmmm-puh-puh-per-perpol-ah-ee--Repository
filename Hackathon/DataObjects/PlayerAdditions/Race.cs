using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.DataObjects.PlayerAdditions;
public class Race
{
	public string name { get; set; }
	public string subtype { get; set; }
	public string size { get; set; }
	public List<string> traits { get; set; }
}