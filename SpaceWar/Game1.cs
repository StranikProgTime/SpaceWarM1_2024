using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceWar.Classes;
using SpaceWar.Classes.SaveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;


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
        public static GameMode gameMode = GameMode.Menu;
        // private Asteroid _asteroid;
        private GameOver _gameOver;
        private MainMenu _mainMenu;
        private PauseMenu _pauseMenu;
        private HUD _hud;
        private HealBoost _heal;

        private Song _gameSong;
        private Song _menuSong;

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
            _hud = new HUD();
            _heal = new HealBoost(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
            );

            _gameOver = new GameOver(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
            );
            _mainMenu = new MainMenu(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
            );
            _pauseMenu = new PauseMenu(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
            );

            _player.TakeDamage += _hud.OnPlayerTakeDamage;
            _player.UpdateScore += _hud.OnScoreUpdated;

            _mainMenu.OnPlayingStarted += OnPlayingStarted;
            _mainMenu.OnLoadGame += LoadGame;
            _pauseMenu.OnPlayingResume += OnPlayingResume;
            _pauseMenu.OnSaveGame += SaveGame;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player.LoadContent(Content);
            _space.LoadContent(Content);
            // _asteroid.LoadContent(Content);
            _hud.LoadContent(GraphicsDevice, Content);
            _heal.LoadContent(Content);

            for (int i = 0; i < COUNT_ASTERIODS; i++)
            {
                LoadAsteroid();
            }

            _gameOver.LoadContent(Content);
            _mainMenu.LoadContent(Content);
            _pauseMenu.LoadContent(Content);

            _gameSong = Content.Load<Song>("gameMusic");
            _menuSong = Content.Load<Song>("menuMusic");

            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_menuSong);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            switch (gameMode)
            {
                case GameMode.Menu:
                    _space.Speed = 0.5f;
                    _space.Update();
                    _mainMenu.Update();
                    break;
                case GameMode.Pause:
                    _space.Speed = 0.5f;
                    _space.Update();
                    _pauseMenu.Update();
                    break;
                case GameMode.Playing:
                    _space.Speed = 3f;
                    _player.Update(
                        _graphics.PreferredBackBufferWidth,
                        _graphics.PreferredBackBufferHeight,
                        Content
                    );
                    _space.Update();
                    _heal.Update();
                    // _asteroid.Update();

                    UpdateAsteroids();
                    CheckCollision();
                    UpdateExplosions(gameTime);

                    if (_player.Health <= 0)
                    {
                        gameMode = GameMode.GameOver;
                        _gameOver.SetScore(_player.Score);

                        MediaPlayer.Play(_menuSong);
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameMode = GameMode.Pause;

                        MediaPlayer.Play(_menuSong);
                    }
                    break;
                case GameMode.GameOver:
                    _space.Speed = 0.5f;
                    _space.Update();
                    _gameOver.Update();
                    break;
                case GameMode.Exit:
                    Exit();
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
                switch (gameMode)
                {
                    case GameMode.Menu:
                        _space.Draw(_spriteBatch);
                        _mainMenu.Draw(_spriteBatch);
                        break;
                    case GameMode.Pause:
                        _space.Draw(_spriteBatch);
                        _pauseMenu.Draw(_spriteBatch);
                        break;
                    case GameMode.Playing:
                        _space.Draw(_spriteBatch);
                        _player.Draw(_spriteBatch);
                        _heal.Draw(_spriteBatch);
                        // _asteroid.Draw(_spriteBatch);

                        foreach (Asteroid asteroid in _asteroids)
                        {
                            asteroid.Draw(_spriteBatch);
                        }

                        foreach (Explosion explosion in _explosions)
                        {
                            explosion.Draw(_spriteBatch);
                        }

                        _hud.Draw(_spriteBatch);
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
                    _player.Damage();

                    CreateExplosion(
                        asteroid.Poisition,
                        asteroid.Width,
                        asteroid.Height
                    );
                }

                // каждый астероид и каждую пулую
                foreach (Bullet bullet in _player.Bullets)
                {
                    if (asteroid.Collision.Intersects(bullet.Collision))
                    {
                        asteroid.IsAlive = false;
                        bullet.IsAlive = false;

                        CreateExplosion(
                            asteroid.Poisition, 
                            asteroid.Width, 
                            asteroid.Height
                        );

                        _player.AddScore();
                    }
                }
            }

            if (_heal.Collision.Intersects(_player.Collision))
            {
                _heal.Reset();
                _player.Heal();
                _hud.OnPlayerHealed();
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

        private void CreateExplosion(Vector2 spawnPosition, int width, int height)
        {
            Explosion explosion = new Explosion(spawnPosition);
            Vector2 position = spawnPosition;
            position = new Vector2(
                position.X - explosion.Width / 2,
                position.Y - explosion.Height / 2
            );
            position = new Vector2(
                position.X + width / 2,
                position.Y + height / 2
            );
            explosion.Position = position;
            explosion.LoadContent(Content);
            _explosions.Add(explosion);

            explosion.PlaySoundEffect();
        }

        private void OnPlayingStarted()
        {
            gameMode = GameMode.Playing;

            MediaPlayer.Play(_gameSong);

            Reset();
        }

        private void OnPlayingResume()
        {
            gameMode = GameMode.Playing;

            MediaPlayer.Play(_gameSong);
        }

        private void Reset()
        {
            _player.Reset();
            _hud.Reset();
            _heal.Reset();

            _explosions.Clear();
            _asteroids.Clear();
        }

        private void SaveGame()
        {
            PlayerData playerData = (PlayerData)_player.SaveData();

            object[] asteroidDatas = new object[_asteroids.Count];

            for (int i = 0; i < _asteroids.Count; i++)
            {
                asteroidDatas[i] = _asteroids[i].SaveData();
            }

            object[] explosionDatas = new object[_explosions.Count];

            for (int i = 0; i < _explosions.Count; i++)
            {
                explosionDatas[i] = _explosions[i].SaveData();
            }

            object[] datas =
            {
                _player.SaveData(),
                _hud.SaveData(),
                asteroidDatas,
                explosionDatas
            };

            string stringData = JsonSerializer.Serialize(datas);

            // using System.IO;

            // Вариант 1
            // StreamWriter writer = new StreamWriter("save.json");
            // writer.WriteLine(stringData);
            // writer.Close();

            // Вариант 2
            File.WriteAllText("save.json", stringData);

            gameMode = GameMode.Menu;
        }

        private void LoadGame()
        {
            if (!File.Exists("save.json"))
            {
                return;
            }

            string jsonString = File.ReadAllText("save.json");
            object[] datas = JsonSerializer.Deserialize<object[]>(jsonString);

            if (datas == null)
            {
                return;
            }

            OnPlayingStarted();

            PlayerData playerData = ((JsonElement)datas[0]).Deserialize<PlayerData>();
            HUDData hudData = ((JsonElement)datas[1]).Deserialize<HUDData>();

            _player.LoadData(playerData, Content);
            _hud.LoadData(hudData, Content);

            object[] array = ((JsonElement)datas[2]).Deserialize<object[]>();

            for (int i = 0; i < array.Length; i++)
            {
                AsteroidData asteroidData = ((JsonElement)array[i]).Deserialize<AsteroidData>();

                Asteroid asteroid = new Asteroid();
                asteroid.LoadData(asteroidData, Content);
                _asteroids.Add(asteroid);
            }

            object[] arrayExplosions = ((JsonElement)datas[3]).Deserialize<object[]>();

            for (int i = 0; i < arrayExplosions.Length; i++)
            {
                ExplosionData explosionData = ((JsonElement)arrayExplosions[i]).Deserialize<ExplosionData>();

                Explosion explosion = new Explosion(Vector2.Zero);
                explosion.LoadData(explosionData, Content);
                _explosions.Add(explosion);
            }
        }
    }
}
