using Cerberus.Shared.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Components
{
	public class BattleComponent : IDrawable, IUpdateable
	{
		//intention is this class handles all meele round computation
		//entites need to register with this component to enter battle
		//battle will draw a box around the player. if able to escape the box the player can flee from battle

		public BattleComponent(IEventAggregator eventAggregator, GraphicsDevice graphics)
		{
			eventAggregator.GetEvent<EnemyEnteredBattleEvent>().Subscribe(OnEnemyEnteredBattle);
			eventAggregator.GetEvent<PlayerEnteredBattleEvent>().Subscribe(OnPlayerEnteredBattle);
		}

		private void OnEnemyEnteredBattle(EnteredBattleArgs obj)
		{
		}

		private void OnPlayerEnteredBattle(EnteredBattleArgs obj)
		{
		}

		public int DrawOrder => 1;

		public bool Visible => true;

		public bool Enabled => true;

		public int UpdateOrder => 1;

		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;

		public void Draw(GameTime gameTime)
		{

		}

		public void Update(GameTime gameTime)
		{

		}
	}
}
