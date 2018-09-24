using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.DataModels
{
	public class CharacterAttributes
	{
		/// <summary>
		/// Gets or sets the strength.
		/// Affects physical attack power, carry capacity, ability to use heavy equipment
		/// </summary>
		/// <value>
		/// The strength.
		/// </value>
		public double Strength { get; set; }

		/// <summary>
		/// Gets or sets the constitution.
		/// Affects hit points, saving throws against strength based skills, resistence against status affects
		/// </summary>
		/// <value>
		/// The constitution.
		/// </value>
		public double Constitution { get; set; }

		/// <summary>
		/// Gets or sets the dexterity.
		/// Affects turn speed, saving throws against dexterity based skills, ability to use light equipment
		/// </summary>
		/// <value>
		/// The dexterity.
		/// </value>
		public double Dexterity { get; set; }

		/// <summary>
		/// Gets or sets the itelligence.
		/// Affects ability to learn magic, ability to weild magic/complex equipment, ability to learn skills
		/// </summary>
		/// <value>
		/// The itelligence.
		/// </value>
		public double Itelligence { get; set; }

		/// <summary>
		/// Gets or sets the wisdom.
		/// Affects saving throws against magic skills, magic slots, magic attack power
		/// </summary>
		/// <value>
		/// The wisdom.
		/// </value>
		public double Wisdom { get; set; }


		public int Moves { get; set; }
		public int HitPoints { get; set; }
		public int Speed { get; set; }
		public int MagicSlots { get; set; }
		public int CarryCapacity { get; set; }
		public int AttackPower { get; set; }
		public int MagicPower { get; set; }
		

	}
}
