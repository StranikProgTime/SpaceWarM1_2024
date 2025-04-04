using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class HealthBar
    {
        private Texture2D _texture;
        private Vector2 _position;
        private int _widthPart;
        private int _numParts;
        private int _height;

        public Rectangle DestinationRectanle
        {
            get {
                return new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _widthPart * _numParts,
                    _height
                ); 
            }
        }

        public int NumParts
        {
            get => _numParts;
            set => _numParts = value;
        }

        public HealthBar(
            Vector2 position, 
            int numParts, 
            int width, 
            int height
        )
        {
            _position = position;
            _numParts = numParts;
            _height = height;

            _widthPart = width / numParts;
        }

        public void LoadContent(GraphicsDevice graphics)
        {
            _texture = new Texture2D(graphics, 1, 1);
            _texture.SetData(new Color[] { Color.Red });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, DestinationRectanle, Color.White);
        }
    }
}
