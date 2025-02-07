using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceWar.Classes;
using System.Collections.Generic;


namespace SpaceWar
{
    public class Game1 : Game
    {
        // Иструменты
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Поля
        private Player _player;
        private Space _space;
        // private Asteroid _asteroid;

        private List<Asteroid> _asteroids;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // _graphics.PreferredBackBufferWidth;
            // _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _player = new Player();
            _space = new Space();
            // _asteroid = new Asteroid();
            _asteroids = new List<Asteroid>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player.LoadContent(Content);
            _space.LoadContent(Content);
            // _asteroid.LoadContent(Content);

            for (int i = 0; i < 10; i++)
            {
                Asteroid asteroid = new Asteroid(new Vector2(i * 40, 0));
                asteroid.LoadContent(Content);

                _asteroids.Add(asteroid);
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _player.Update(
                _graphics.PreferredBackBufferWidth, 
                _graphics.PreferredBackBufferHeight
            );
            _space.Update();
            // _asteroid.Update();

            foreach (Asteroid asteroid in _asteroids)
            {
                asteroid.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            {
                _space.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                // _asteroid.Draw(_spriteBatch);

                foreach (Asteroid asteroid in _asteroids)
                {
                    asteroid.Draw(_spriteBatch);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
