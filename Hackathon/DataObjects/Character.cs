using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.DataObjects;
public  class Character
{
	public struct Player {
		public String name;
		public int id;
	}

	public struct Race
	{
		public String name;
		public String subtype;
		public String size;
		public String[] traits;
	}

	public struct Classes
	{
		public String name;
		public int level;
		public String subtype;
	}

	public struct Background
	{
		public String name;
		public String description;
	}

	public struct Details
	{
		public int age;
		public String eyes;
		public String skin;
		public float weight;
		public String personality;
		public String ideal;
		public String bond;
		public String flaw;
		public String backstory;
		public String physical;
	}

	public struct AbilityScores
	{
		int str;
		int dex;
		int con;
		int intt;
		int wis;
		int cha;
	}

	public Player CharacterOwner { get; set; }
	public Race CharacterRace{get; set;}
	public Classes CharacterClasses{get; set;}
	public Background CharacterBackground{get; set;}
	public Details CharacterDetails{get; set;}
	public float CharacterAlignment{get; set;}
	public AbilityScores CharacterAbilityScores{get; set;}
	public String[] Languages { get; set; }
	public float Gold { get; set; }
	String Name { get; set; }

	// Inventory
	// Spells?!??!!?


}
