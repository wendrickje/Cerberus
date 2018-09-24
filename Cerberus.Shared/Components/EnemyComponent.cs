using Cerberus.Shared.DataModels;
using Cerberus.Shared.Entities;
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
	public class EnemyComponent : IDrawable, IUpdateable, IEntity
	{
		const int TileHeight = 32;
		const int TileWidth = 32;
		private SpriteBatch _spriteBatch;
		private Texture2D _texture;
		IEventAggregator _eventAggregator;

		Point _position;
		private bool _isEngaged;

		public Point GetPosition() { return _position; }

		public CharacterAttributes Attributes { get; set; }
		public EntityType EntityType { get; } = EntityType.Enemy;
		public Guid Id { get; private set; }

		public EnemyComponent(IEventAggregator eventAggregator, GraphicsDevice graphics, Texture2D texture)
		{
			_eventAggregator = eventAggregator;
			_texture = texture;
			_spriteBatch = new SpriteBatch(graphics);
			Attributes = new CharacterAttributes();
			Id = Guid.NewGuid();
		}


		public bool Enabled => true;

		public int UpdateOrder => 2;

		public int DrawOrder => 2;

		public bool Visible => true;

		public bool IsAlive { get; internal set; } = true;

		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;

		public void SetPosition(Point point)
		{
			_position = point;
			_initialPosition = point;
			_eventAggregator.GetEvent<UpdateLogEvent>().Publish(new UpdateLogArgs() { Text = $"enemy postion: {{{_position.X / TileWidth}, {_position.Y / TileHeight}}}" });
			_eventAggregator.GetEvent<EntityPositionChangedEvent>().Publish(new EntityPositionChangedArgs() { Entity = this });

		}

		public void Draw(GameTime gameTime)
		{

			_spriteBatch.Begin();

			_spriteBatch.Draw(_texture, _position.ToVector2(), Color.White);


			_spriteBatch.End();
		}


		int _frames = 0;
		int _steps = 0;
		private Point _initialPosition;

		public void Update(GameTime gameTime)
		{
			/*
			var xmin = 2;
			var ymin = 2;
			var mouse = Mouse.GetState();

			var currentTileX = (int)Math.Round((double)(mouse.X / TileWidth));
			var currentTileY = (int)Math.Round((double)(mouse.Y / TileHeight));

			var rightBounds = Math.Min(((TileColumns+1) * TileWidth), currentTileX * TileWidth);
			var bottomBounds = Math.Min(((TileRows +1)* TileHeight), currentTileY * TileHeight);
			_playerPosition = new Point(Math.Max(xmin * TileWidth, rightBounds), Math.Max(ymin * TileHeight, bottomBounds));
			*/
			if (_isEngaged)
			{
				if(_frames >= 30)
				{
					_position += new Point(0, 8);
					_frames = 0;
					_steps++;
					return;
				}
				if(_steps == 2)
				{
					_position = _initialPosition;
					_steps = 0;
				}
				_frames++;
			}
		}

		internal void EngagePlayer(Point playerPosition)
		{

			_isEngaged = true;
		}
	}

}
