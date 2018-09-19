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

			_tileTexture = tileTexture;

			_hoverTileTexture = new Texture2D(graphics, TileWidth - 1, TileHeight - 1);

			Color[] data = new Color[_hoverTileTexture.Width * _hoverTileTexture.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Gold;
			_hoverTileTexture.SetData(data);
		}



		public void Draw(GameTime gameTime)
		{

			_spriteBatch.Begin();
			//draw tiles
			for (int y = paddingY; y < TileRows + paddingY; y++)
				for (int x = paddingX; x < TileColumns + paddingX; x++)
				{

					_spriteBatch.Draw(_tileTexture, new Rectangle(new Point(x * TileWidth, y * TileHeight), new Point(TileWidth, TileHeight)), Color.White);
					if (
						_mousePosition.X >= (x * TileWidth) && _mousePosition.X < (x * TileWidth) + TileWidth &&
						_mousePosition.Y >= (y * TileHeight) && _mousePosition.Y < (y * TileHeight) + TileHeight
						)
					{

						_spriteBatch.Draw(_hoverTileTexture, new Rectangle(new Point((x * TileWidth), (y * TileHeight)+1), new Point(TileWidth-1, TileHeight-1)), new Color(128, 128, 128, 128));
					}
					//else
				}

			_spriteBatch.End();
		}


		bool _ismousedown;
		bool _wasmousedown;

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
				var point = new Point(Math.Max(paddingX * TileWidth, rightBounds), Math.Max(paddingY * TileHeight, bottomBounds));
				_wasmousedown = false;
				_eventAggregator.GetEvent<TileSelectedEvent>().Publish(new TileSelectedArgs() { Tile = point });
			}

		}
	}
}
