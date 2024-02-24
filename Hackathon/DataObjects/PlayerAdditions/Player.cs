using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.DataObjects.PlayerAdditions;
public class Player
{
	public string name { get; set; }
	public string discordId { get; set; }// ulong -> string because ulong be to long
}