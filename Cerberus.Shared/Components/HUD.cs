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
	public class HUD : IDrawable, IUpdateable
	{
		IEventAggregator _eventAggregator;
		Texture2D _texture;
		GraphicsDevice _graphics;
		private SpriteBatch _spriteBatch;
		SpriteFont _font;
		public HUD(IEventAggregator eventAggregator, GraphicsDevice graphics, Texture2D texture, SpriteFont font)
		{
			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<UpdateLogEvent>().Subscribe(OnLogUpdated);
			_texture = texture;
			_graphics = graphics;
			_spriteBatch = new SpriteBatch(graphics);
			_font = font;
		}

		private void OnLogUpdated(UpdateLogArgs obj)
		{
			// check for word wrapping
			// and scroll
			if(Log.Count == 10)
			{
				Log.RemoveAt(0);
			}
			Log.Add(obj.Text);
		}

		private string GetLog()
		{
			return String.Join("\r\n", Log);
		}

		public int DrawOrder => 1;

		public bool Visible => true;

		public bool Enabled => true;

		public int UpdateOrder => 1;

		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;


		public List<string> Log { get; set; } = new List<string>();


		public void Draw(GameTime gameTime)
		{
			_spriteBatch.Begin();

			_spriteBatch.Draw(_texture, new Vector2(32 * 2, 32 * 10), Color.White);
			_spriteBatch.DrawString(_font, GetLog(), new Vector2(32 * 2, 32 * 10), Color.White);

			_spriteBatch.End();
		}

		public void Update(GameTime gameTime)
		{

		}
	}
}
