using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class Bullet
    {
        private Texture2D _texture;

        private int _width = 20;
        private int _height = 20;
        private int _speed = 3;

        private Rectangle _destinationRectangle;

        // Свойства
        public Vector2 Position
        {
            set
            {
                _destinationRectangle.X = (int)value.X;
                _destinationRectangle.Y = (int)value.Y;
            }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Bullet()
        {
            _texture = null;
            _destinationRectangle = new Rectangle(100, 300, _width, _height);
        }

        public void LoadContent(ContentManager content)
        {
            // TODO: поменять текстуру
            _texture = content.Load<Texture2D>("asteroid"); 
        }

        public void Update()
        {
            _destinationRectangle.Y -= _speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _destinationRectangle, Color.White);
        }
    }
}
