using Cerberus.Shared.Entities;
using Cerberus.Shared.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Components
{
	public class MapComponent : IDrawable, IUpdateable
	{
		const int TileHeight = 32;
		const int TileWidth = 32;
		const int TileRows = 8;
		const int TileColumns = 21;
		const int paddingY = 2;
		const int paddingX = 2;
		private SpriteBatch _spriteBatch;
		Texture2D _tileTexture;
		private IEventAggregator _eventAggregator;
		GraphicsDevice _graphicsDevice;

		Dictionary<Point, IEntity> _entities;

		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;

		public bool Enabled => true;

		public int UpdateOrder => 1;

		public int DrawOrder => 1;

		public bool Visible => true;

		private Point _mousePosition = new Point();
		Texture2D _hoverTileTexture;

		public MapComponent(IEventAggregator eventAggregator, GraphicsDevice graphics, Texture2D tileTexture)
		{
			_eventAggregator = eventAggregator;
			_graphicsDevice = graphics;
			_spriteBatch = new SpriteBatch(graphics);
			_entities = new Dictionary<Point, IEntity>();
			_tileTexture = tileTexture;

			_hoverTileTexture = new Texture2D(graphics, TileWidth - 1, TileHeight - 1);
			Color[] data = new Color[_hoverTileTexture.Width * _hoverTileTexture.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Gold;
			_hoverTileTexture.SetData(data);

			_eventAggregator.GetEvent<EntityPositionChangedEvent>().Subscribe(OnEntityPositionChanged);
		}

		private void OnEntityPositionChanged(EntityPositionChangedArgs obj)
		{
			var entityPosition = obj.Entity.GetPosition();
			var existing = _entities.Values.FirstOrDefault(e => e.Id == obj.Entity.Id);
			if(existing != null)
			{
				_entities.Remove(new Point(entityPosition.X / TileWidth, entityPosition.Y / TileHeight));
			
			}
			var key = new Point(entityPosition.X / TileWidth, entityPosition.Y / TileHeight);
			
			_entities.Add(key, obj.Entity);
			
			
			// check for entities around the player
			var entities = _entities.Where(kvp => kvp.Value.EntityType != EntityType.Player && IsWithinOneTile(kvp.Key, _playerPosition));

			//all enemies around
			foreach (var enemy in entities.Select(e => e.Value).OfType<EnemyComponent>())
			{
				var enemyPosition = enemy.GetPosition();
				_eventAggregator.GetEvent<UpdateLogEvent>().Publish(new UpdateLogArgs() { Text = $"around player: {{{enemyPosition.X / TileWidth}, {enemyPosition.Y / TileHeight}}}" });
				enemy.EngagePlayer(_playerPosition);
				

			}

		}

		public void Draw(GameTime gameTime)
		{

			_spriteBatch.Begin();
			//draw tiles
			for (int y = paddingY; y < TileRows + paddingY; y++)
				for (int x = paddingX; x < TileColumns + paddingX; x++)
				{
					//draw tiles
					_spriteBatch.Draw(_tileTexture, new Rectangle(new Point(x * TileWidth, y * TileHeight), new Point(TileWidth, TileHeight)), Color.White);

					//draw highlighted tile
					if (_mousePosition.X >= (x * TileWidth) && _mousePosition.X < (x * TileWidth) + TileWidth &&
						_mousePosition.Y >= (y * TileHeight) && _mousePosition.Y < (y * TileHeight) + TileHeight)
					{

						_spriteBatch.Draw(_hoverTileTexture, new Rectangle(new Point((x * TileWidth), (y * TileHeight)+1), new Point(TileWidth-1, TileHeight-1)), new Color(128, 128, 128, 128));
					}
				}

			_spriteBatch.End();
		}


		bool _ismousedown;
		bool _wasmousedown;
		Point _playerPosition;

		public void Update(GameTime gameTime)
		{
			var mouse = Mouse.GetState();
			_mousePosition = mouse.Position;
			if(mouse.LeftButton == ButtonState.Pressed)
			{
				_ismousedown = true;
			}
			if(_ismousedown && mouse.LeftButton == ButtonState.Released)
			{
				_ismousedown = false;
				_wasmousedown = true;
			}
			if (_wasmousedown && !_ismousedown)
			{


				var currentTileX = (int)Math.Round((double)(mouse.X / TileWidth));
				var currentTileY = (int)Math.Round((double)(mouse.Y / TileHeight));

				var rightBounds = Math.Min(((TileColumns + 1) * TileWidth), currentTileX * TileWidth);
				var bottomBounds = Math.Min(((TileRows + 1) * TileHeight), currentTileY * TileHeight);
				var targetPosition = new Point(Math.Max(paddingX * TileWidth, rightBounds), Math.Max(paddingY * TileHeight, bottomBounds));
				// only move player if tile is not occupied
				if (!_entities.Any(kvp => kvp.Key == new Point(targetPosition.X / TileWidth, targetPosition.Y / TileHeight) && kvp.Value.EntityType != EntityType.Player))
				{
					_playerPosition = targetPosition;
					_eventAggregator.GetEvent<TileSelectedEvent>().Publish(new TileSelectedArgs() { Tile = _playerPosition });
				}
				_wasmousedown = false;
			}


		}

		private bool IsWithinOneTile(Point entityTile, Point playerPosition)
		{
			//using Point just for convenience
			var playerTile = new Point(playerPosition.X / TileWidth, playerPosition.Y / TileHeight);

			// [ ] [ ] [o]
			// [ ] [x] [ ]
			// [ ] [ ] [ ]
			// calculate possible positions which are 1 tile around player
			var possiblePositions = new List<Point>();
			for (int y = playerTile.Y - 1; y <= playerTile.Y + 1; y++)
				for (int x = playerTile.X - 1; x <= playerTile.X + 1; x++)
				{
					if (x == playerTile.X && y == playerTile.Y) continue;
					possiblePositions.Add(new Point(x, y));
				}
			if(possiblePositions.Contains(entityTile))
			{
				
				return true;
			}
			return false;
		}
	}
}
