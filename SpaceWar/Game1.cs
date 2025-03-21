using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceWar.Classes;
using System;
using System.Collections.Generic;


namespace SpaceWar
{
    public class Game1 : Game
    {
        // Константы
        private const int COUNT_ASTERIODS = 10;

        // Иструменты
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Поля
        private Player _player;
        private Space _space;
        private Label _label;
        private GameMode _gameMode = GameMode.Menu;
        // private Asteroid _asteroid;

        private GameOver _gameOver;
        private MainMenu _mainMenu;


        private List<Asteroid> _asteroids;
        private List<Explosion> _explosions;

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
            _explosions = new List<Explosion>();

            _gameOver = new GameOver(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
            );
            _mainMenu = new MainMenu();

            _label = new Label(Vector2.Zero, "Hello world!", Color.White);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player.LoadContent(Content);
            _space.LoadContent(Content);
            // _asteroid.LoadContent(Content);

            for (int i = 0; i < COUNT_ASTERIODS; i++)
            {
                LoadAsteroid();
            }

            _label.LoadContent(Content);
            _gameOver.LoadContent(Content);
            _mainMenu.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (_gameMode)
            {
                case GameMode.Menu:
                    _mainMenu.Update();
                    break;
                case GameMode.Playing:
                    _player.Update(
                        _graphics.PreferredBackBufferWidth,
                        _graphics.PreferredBackBufferHeight,
                        Content
                    );
                    _space.Update();
                    // _asteroid.Update();

                    UpdateAsteroids();
                    CheckCollision();
                    UpdateExplosions(gameTime);
                    break;
                case GameMode.GameOver:
                    _space.Update();
                    _gameOver.Update();
                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            {
                switch (_gameMode)
                {
                    case GameMode.Menu:
                        _mainMenu.Draw(_spriteBatch);
                        break;
                    case GameMode.Playing:
                        _space.Draw(_spriteBatch);
                        _player.Draw(_spriteBatch);
                        // _asteroid.Draw(_spriteBatch);

                        foreach (Asteroid asteroid in _asteroids)
                        {
                            asteroid.Draw(_spriteBatch);
                        }

                        foreach (Explosion explosion in _explosions)
                        {
                            explosion.Draw(_spriteBatch);
                        }

                        _label.Draw(_spriteBatch);
                        break;
                    case GameMode.GameOver:
                        _space.Draw(_spriteBatch);
                        _gameOver.Draw(_spriteBatch);
                        break;
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateAsteroids()
        {
            for (int i = 0; i < _asteroids.Count; i++)
            {
                Asteroid asteroid = _asteroids[i];

                asteroid.Update();

                // teleport
                if (asteroid.Poisition.Y > _graphics.PreferredBackBufferHeight)
                {
                    Random random = new Random();

                    int x = random.Next(0, _graphics.PreferredBackBufferWidth - asteroid.Width);
                    int y = random.Next(0, _graphics.PreferredBackBufferHeight);

                    asteroid.Poisition = new Vector2(x, -y);
                }

                // check isAlive asteroid
                if (!asteroid.IsAlive)
                {
                    _asteroids.Remove(asteroid);
                    i--;
                }
            }

            // Загружаем доп. астероиды в игру
            if (_asteroids.Count < COUNT_ASTERIODS)
            {
                LoadAsteroid();
            }
        }

        private void LoadAsteroid()
        {
            Asteroid asteroid = new Asteroid();
            asteroid.LoadContent(Content);

            Random random = new Random();

            int x = random.Next(0, _graphics.PreferredBackBufferWidth - asteroid.Width);
            int y = random.Next(0, _graphics.PreferredBackBufferHeight);

            asteroid.Poisition = new Vector2(x, -y);

            _asteroids.Add(asteroid);
        }

        private void CheckCollision()
        {
            foreach (Asteroid asteroid in _asteroids)
            {
                // каждый астероид и игрока
                if (asteroid.Collision.Intersects(_player.Collision))
                {
                    asteroid.IsAlive = false;

                    Explosion explosion = new Explosion(asteroid.Poisition);
                    Vector2 position = asteroid.Poisition;
                    position = new Vector2(
                        position.X - explosion.Width / 2,
                        position.Y - explosion.Height / 2
                    );
                    position = new Vector2(
                        position.X + asteroid.Width / 2,
                        position.Y + asteroid.Height / 2
                    );
                    explosion.Position = position;
                    explosion.LoadContent(Content);
                    _explosions.Add(explosion);
                }

                // каждый астероид и каждую пулую
                foreach (Bullet bullet in _player.Bullets)
                {
                    if (asteroid.Collision.Intersects(bullet.Collision))
                    {
                        asteroid.IsAlive = false;
                        bullet.IsAlive = false;

                        Explosion explosion = new Explosion(asteroid.Poisition);
                        Vector2 position = asteroid.Poisition;
                        position = new Vector2(
                            position.X - explosion.Width / 2,
                            position.Y - explosion.Height / 2
                        );
                        position = new Vector2(
                            position.X + asteroid.Width / 2,
                            position.Y + asteroid.Height / 2
                        );
                        explosion.Position = position;
                        explosion.LoadContent(Content);
                        _explosions.Add(explosion);
                    }
                }
            }
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = 0; i < _explosions.Count; i++)
            {
                _explosions[i].Update(gameTime);

                if (!_explosions[i].IsAlive)
                {
                    _explosions.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
