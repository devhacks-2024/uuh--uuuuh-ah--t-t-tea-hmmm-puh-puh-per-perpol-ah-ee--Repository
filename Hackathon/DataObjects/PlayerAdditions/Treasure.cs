using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.DataObjects.PlayerAdditions;
public class Treasure
{
	public int gold { get; set; }
	public int silver { get; set; }
	public int copper { get; set; }

	public Treasure()
	{
		gold= 0;
		silver = 0;
		copper= 0;
	}


	// ComplexParameters allow for this to be a parameter in a discord command
	[ComplexParameterCtor]
	public Treasure(int gold, int silver, int copper)
	{
		this.gold = gold;
		this.silver = silver;
		this.copper = copper;
	}

	public static Treasure operator +(Treasure a, Treasure b)
	{
		return new Treasure(a.gold +b.gold, a.silver+b.silver, a.copper+b.copper);
	}

	public override string ToString()
	{
		return $"Gold:{gold} Silver:{silver} Copper:{copper}";
	}

}