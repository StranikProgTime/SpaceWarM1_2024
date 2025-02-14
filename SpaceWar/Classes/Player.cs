using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class Player
    {
        private Vector2 _position;
        private Texture2D _texture;
        private float _speed;

        private Rectangle _collision;

        public Rectangle Collision
        {
            get { return _collision; }
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

        public void Update(int widthScreen, int heightScreen)
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
