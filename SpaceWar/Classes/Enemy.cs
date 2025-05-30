using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace SpaceWar.Classes
{
    public class Enemy
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _collision;
        private int _heightScreen;

        private float _speed = 2.0f;

        private int _timer = 0;
        private int _maxTime = 50;

        private List<Bullet> _bulletList;

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public int Width
        {
            get => _texture.Width;
        }

        public int Height
        {
            get => _texture.Height;
        }

        public Rectangle Collision
        {
            get => _collision;
        }

        public List<Bullet> BulletList
        {
            get => _bulletList;
        }

        public bool IsAlive;

        public Enemy(int heightScreen)
        {
            _texture = null;
            IsAlive = true;

            _bulletList = new List<Bullet>();

            _heightScreen = heightScreen;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("enemy");
        }

        public void Update(ContentManager content)
        {
            _position.Y += _speed; // _position.Y = _position.Y + _speed;

            if (_position.Y > _heightScreen)
            {
                IsAlive = false;
            }

            _timer++;

            if (_timer >= _maxTime)
            {
                Bullet bullet = new Bullet(
                    new Vector2(0, 4),
                    "enemyBullet",
                    null,
                    _heightScreen
                );
                bullet.Position = new Vector2(
                    _position.X + _texture.Width / 2 - bullet.Width / 2,
                    _position.Y + _texture.Height - bullet.Height / 2
                );

                bullet.LoadContent(content);

                _bulletList.Add(bullet);
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

            _collision = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                Width,
                Height
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);

            foreach (Bullet bullet in _bulletList)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
