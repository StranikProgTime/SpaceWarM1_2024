using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceWar.Classes.SaveData;

namespace SpaceWar.Classes
{
    public class Bullet : ISaveable
    {
        private Texture2D _texture;

        private int _width = 20;
        private int _height = 20;
        private Vector2 _velocity;
        private string _bulletTexture;
        private string _soundEffectName;
        private int _heightScreen;

        private Rectangle _destinationRectangle;

        private bool _isAlive;

        private SoundEffect _soundEffect;

        // Свойства
        public Vector2 Position
        {
            get
            {
                return new Vector2(_destinationRectangle.X, _destinationRectangle.Y);
            }
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

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public Rectangle Collision
        {
            get { return _destinationRectangle; }
        }

        public Bullet(
            Vector2 velocity, 
            string bulletTexture, 
            string soundEffectName,
            int heightScreen
        )
        {
            _texture = null;
            _isAlive = true;
            _destinationRectangle = new Rectangle(100, 300, _width, _height);

            _velocity = velocity;
            _bulletTexture = bulletTexture;
            _soundEffectName = soundEffectName;
            _heightScreen = heightScreen;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_bulletTexture);

            if (_soundEffectName != null)
            {
                _soundEffect = content.Load<SoundEffect>(_soundEffectName);
            }
        }

        public void Update()
        {
            _destinationRectangle.Y += (int)_velocity.Y;
            _destinationRectangle.X += (int)_velocity.X;

            if (_destinationRectangle.Y <= 0 - _height)
            {
                _isAlive = false;
            }

            if (_destinationRectangle.Y >= _heightScreen)
            {
                _isAlive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _destinationRectangle, Color.White);
        }

        public void PlaySoundEffect()
        {
            if (_soundEffect == null)
            {
                return;
            }

            SoundEffectInstance instance = _soundEffect.CreateInstance();

            instance.Volume = 0.01f;
            instance.Play();
        }

        public object SaveData()
        {
            BulletData data = new BulletData();
            data.X = (int)Position.X;
            data.Y = (int)Position.Y;
            data.IsAlive = _isAlive;

            return data;
        }

        public void LoadData(object data, ContentManager content)
        {
            if (!(data is BulletData))
            {
                return;
            }

            BulletData bulletData = (BulletData)data;

            Position = new Vector2(bulletData.X, bulletData.Y);
            _isAlive = bulletData.IsAlive;
        }
    }
}
