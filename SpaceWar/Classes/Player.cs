using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SpaceWar.Classes.SaveData;

namespace SpaceWar.Classes
{
    public class Player : ISaveable
    {
        private Vector2 _position;
        private Texture2D _texture;
        private float _speed;

        private int _health = 10;
        private int _score = 0;

        private Rectangle _collision;

        // weapon
        private List<Bullet> _bulletList = new List<Bullet>(); // магазин патронов

        // timer
        private int _timer = 0;
        private int _maxTime = 10;

        // events
        public event Action<int> TakeDamage;
        public event Action<int> UpdateScore;

        public Rectangle Collision
        {
            get { return _collision; }
        }

        public List<Bullet> Bullets
        {
            get { return _bulletList; }
        }

        public int Health
        {
            get => _health;
        }

        public int Score
        {
            get => _score;
        }

        public Player()
        {
            _position = new Vector2(30, 30);
            _texture = null;

            _speed = 7;
            _collision = new Rectangle((int)_position.X, (int)_position.Y, 0, 0);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("player");
        }

        public void Update(int widthScreen, int heightScreen, ContentManager content)
        {
            KeyboardState keyboard = Keyboard.GetState();

            #region Movment
            if (keyboard.IsKeyDown(Keys.S))
            {
                _position.Y += _speed;
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                _position.Y -= _speed;
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                _position.X -= _speed;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                _position.X += _speed;
            }
            #endregion

            #region Bounds
            if (_position.X < 0)
            {
                _position.X = 0;
            }

            if (_position.X > widthScreen - _texture.Width)
            {
                _position.X = widthScreen - _texture.Width;
            }

            if (_position.Y < 0)
            {
                _position.Y = 0;
            }

            if (_position.Y > heightScreen - _texture.Height)
            {
                _position.Y = heightScreen - _texture.Height;
            }
            #endregion

            _collision = new Rectangle(
                (int)_position.X, 
                (int)_position.Y, 
                _texture.Width, 
                _texture.Height
            );

            if (_timer <= _maxTime)
            {
                _timer++;
            }

            if (keyboard.IsKeyDown(Keys.Space) && _timer >= _maxTime)
            {
                Bullet bullet = new Bullet();
                bullet.Position = new Vector2(
                    _position.X + _texture.Width / 2 - bullet.Width / 2,
                    _position.Y - bullet.Height / 2
                );

                bullet.LoadContent(content);

                _bulletList.Add(bullet);
                bullet.PlaySoundEffect();
                _timer = 0;
            }

            // работа со всем пульками в игре
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Update();
            }

            for (int i = 0; i < _bulletList.Count; i++)
            {
                if (_bulletList[i].IsAlive == false)
                {
                    _bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);

            foreach (Bullet bullet in _bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void Damage()
        {
            _health--;

            if (TakeDamage != null)
            {
                TakeDamage(_health);
            }
        }

        public void Heal()
        {
            _health = 10;
        }

        public void AddScore()
        {
            _score++;

            if (UpdateScore != null)
            {
                UpdateScore(_score);
            }
        }

        public void Reset()
        {
            _position = new Vector2(350, 400);
            _score = 0;
            _health = 10;

            _bulletList.Clear();
        }

        public object SaveData()
        {
            List<BulletData> bullets = new List<BulletData>();

            foreach (var bullet in _bulletList)
            {
                bullets.Add((BulletData)bullet.SaveData());
            }

            PlayerData data = new PlayerData();
            data.X = (int)_position.X;
            data.Y = (int)_position.Y;
            data.Health = _health;
            data.Score = _score;
            data.Timer = _timer;
            data.Bullets = bullets;

            return data;
        }

        public void LoadData(object data, ContentManager content)
        {
            if (!(data is PlayerData))
            {
                return;
            }

            PlayerData playerData = (PlayerData)data;

            _position = new Vector2(playerData.X, playerData.Y);
            _health = playerData.Health;
            _score = playerData.Score;
            _timer = playerData.Timer;

            foreach (var bullet in playerData.Bullets)
            {
                Bullet bull = new Bullet();
                bull.LoadData(bullet, content);
                bull.LoadContent(content);

                _bulletList.Add(bull);
            }
        }
    }
}
