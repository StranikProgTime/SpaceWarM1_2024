using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SpaceWar.Classes
{
    public class Explosion
    {
        private Texture2D _texture; // 1983 x 117
        private Vector2 _position;

        private double _time = 0.0d;
        private double _duration = 30.0d;

        private int _frameNumber = 0;
        private int _widthFrame = 117;
        private int _heightFrame = 117;

        private Rectangle _sourceRectangle; // нужно для рисования области текстуры

        public bool IsAlive { get; set; } = true;

        public Explosion(Vector2 position)
        {
            _texture = null;
            _position = position;

            _sourceRectangle = new Rectangle(
                _frameNumber * _widthFrame,
                0,
                _widthFrame,
                _heightFrame
            );
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("explosion3");
        }

        public void Update(GameTime gameTime)
        {
            _time += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_time >= _duration)
            {
                _frameNumber++;
                _time = 0;
            }

            if (_frameNumber == 17)
            {
                IsAlive = false;
            }

            _sourceRectangle = new Rectangle(
                _frameNumber * _widthFrame,
                0,
                _widthFrame,
                _heightFrame
            );

            Debug.WriteLine("Time: " + gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White);
        }
    }
}
