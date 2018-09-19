using Cerberus.Shared.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Prism.Events;

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

			var tileTexture = Content.Load<Texture2D>("Tile");
			Map = new MapComponent(_eventAggregator, GraphicsDevice, tileTexture);
			var playerTexture = Content.Load<Texture2D>("Warrior");
			Player = new PlayerComponent(_eventAggregator, GraphicsDevice, playerTexture);
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
			Player.Update(gameTime);
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
			Player.Draw(gameTime);
			base.Draw(gameTime);
			_spriteBatch.End();
		}
	}
}
