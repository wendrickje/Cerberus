using Cerberus.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;

namespace Cerberus.Shared
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class CerberusGame : Game
	{
		GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;
		public MapComponent Map { get; set; }
		public PlayerComponent Player { get; set; }
		IEventAggregator _eventAggregator;
		EnemyComponent Enemy;
		EnemyComponent Enemy2;
		HUD _hud;
		List<EnemyComponent> _enemies = new List<EnemyComponent>();

		public CerberusGame()
		{
			_eventAggregator = new EventAggregator();
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			
			this.IsMouseVisible = true;
			_graphics.IsFullScreen = false;
			_graphics.PreferredBackBufferWidth = 800;
			_graphics.PreferredBackBufferHeight = 480;
			_graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);


			var hud = new Texture2D(GraphicsDevice, 400, 200);
			var data = new Color[hud.Width * hud.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
			hud.SetData(data);
			var font = Content.Load<SpriteFont>("font");
			_hud = new HUD(_eventAggregator, GraphicsDevice, hud, font);

			var tileTexture = Content.Load<Texture2D>("Tile");
			Map = new MapComponent(_eventAggregator, GraphicsDevice, tileTexture);

			var playerTexture = Content.Load<Texture2D>("Warrior");
			Player = new PlayerComponent(_eventAggregator, GraphicsDevice, playerTexture);

			var enemy = new Texture2D(GraphicsDevice, 16, 16);
			data = new Color[enemy.Width * enemy.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
				enemy.SetData(data);
			Enemy = new EnemyComponent(_eventAggregator, GraphicsDevice, enemy);
			Enemy.SetPosition(new Point(32 * 5, 32 * 4));


			var enemy2 = new Texture2D(GraphicsDevice, 16, 16);
			data = new Color[enemy2.Width * enemy2.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Blue;
			enemy2.SetData(data);
			Enemy2 = new EnemyComponent(_eventAggregator, GraphicsDevice, enemy2);
			Enemy2.SetPosition(new Point(32 * 9, 32 * 7));

			_enemies.Add(Enemy);
			_enemies.Add(Enemy2);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();
			Map.Update(gameTime);
			_hud.Update(gameTime);
			Player.Update(gameTime);
			foreach(var enemy in _enemies.Where(e => e.IsAlive))
			{
				enemy.Update(gameTime);
			}
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			_spriteBatch.Begin();
			_spriteBatch.GraphicsDevice.Clear(Color.DarkSlateGray);
			Map.Draw(gameTime);
			_hud.Draw(gameTime);
			Player.Draw(gameTime);
			foreach (var enemy in _enemies.Where(e => e.IsAlive))
			{
				enemy.Draw(gameTime);
			}
			base.Draw(gameTime);
			_spriteBatch.End();
		}
	}
}
