using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class Space
    {
        private Texture2D _texture;
        private Vector2 _position1;
        private Vector2 _position2;
        private float _speed;

        public float Speed
        {
            set => _speed = value;
        }

        public Space()
        {
            _texture = null;
            _position1 = Vector2.Zero;
            _position2 = Vector2.Zero; // new Vector2(0, 0);
            _speed = 3;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("space");

            _position1 = new Vector2(0, -_texture.Height);
        }

        public void Update()
        {
            _position1.Y += _speed;
            _position2.Y += _speed;

            if (_position1.Y >= 0)
            {
                _position1.Y = -_texture.Height;
                _position2.Y = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position1, Color.White);
            spriteBatch.Draw(_texture, _position2, Color.White);
        }
    }
}
