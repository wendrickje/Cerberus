﻿using Cerberus.Shared.DataModels;
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
	public class PlayerComponent : IDrawable, IUpdateable, IEntity
	{
		const int TileHeight = 32;
		const int TileWidth = 32;
		IEventAggregator _eventAggregator;

		private SpriteBatch _spriteBatch;
		private Texture2D _playerTexture;

		Point _position;
		public Point GetPosition() { return _position; }

		public CharacterAttributes Attributes { get; set; }
		public EntityType EntityType { get; } = EntityType.Player;

		public PlayerComponent(IEventAggregator eventAggregator, GraphicsDevice graphics, Texture2D playerTexture)
		{
			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<TileSelectedEvent>().Subscribe(OnTileSelected);
			_playerTexture = playerTexture;
			_spriteBatch = new SpriteBatch(graphics);
			Attributes = new CharacterAttributes();
			Id = Guid.NewGuid();
			SetPosition(new Point());
		}

		private void OnTileSelected(TileSelectedArgs obj)
		{
			SetPosition(obj.Tile);
		}

		public void SetPosition(Point point)
		{
			_position = point;
			_eventAggregator.GetEvent<UpdateLogEvent>().Publish(new UpdateLogArgs() { Text = $"player postion: {{{_position.X / TileWidth}, {_position.Y / TileHeight}}}" });
			_eventAggregator.GetEvent<EntityPositionChangedEvent>().Publish(new EntityPositionChangedArgs() { Entity = this });

		}

		public bool Enabled => true;

		public int UpdateOrder => 2;

		public int DrawOrder => 2;

		public bool Visible => true;

		public Guid Id { get; private set; }

		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;

		public void Draw(GameTime gameTime)
		{

			_spriteBatch.Begin();

			_spriteBatch.Draw(_playerTexture, _position.ToVector2(), Color.White);
			

			_spriteBatch.End();
		}

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
		}
	}
}
