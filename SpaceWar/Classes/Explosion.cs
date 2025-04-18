using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

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

        private SoundEffect _soundEffect;   // Добавление звука взрыва

        public bool IsAlive { get; set; } = true;

        public int Width
        {
            get { return _widthFrame; }
        }

        public int Height
        {
            get { return _heightFrame; }
        }

        public Vector2 Position
        {
            set { _position = value; }
        }

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

            _soundEffect = content.Load<SoundEffect>("explosion");
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

        public void PlaySoundEffect()
        {
            // Вариант 1
            // _soundEffect.Play();

            // Вариант 2
            SoundEffectInstance instance = _soundEffect.CreateInstance();

            instance.Volume = 0.001f;
            instance.Play();
        }
    }
}
