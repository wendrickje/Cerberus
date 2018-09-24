using Cerberus.Shared.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Events
{
	public class EnemyEnteredBattleEvent : PubSubEvent<EnteredBattleArgs>
	{
	}
	public class PlayerEnteredBattleEvent : PubSubEvent<EnteredBattleArgs>
	{
	}

	public class EnteredBattleArgs
	{
		public IEntity Competitor { get; set; }
	}
}
