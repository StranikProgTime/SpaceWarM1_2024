﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private int _speed = 4;

        private Rectangle _destinationRectangle;

        private bool _isAlive;

        private SoundEffect _soundEffect;

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

        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public Rectangle Collision
        {
            get { return _destinationRectangle; }
        }

        public Bullet()
        {
            _texture = null;
            _isAlive = true;
            _destinationRectangle = new Rectangle(100, 300, _width, _height);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("bullet");

            _soundEffect = content.Load<SoundEffect>("laserFire");
        }

        public void Update()
        {
            _destinationRectangle.Y -= _speed;

            if (_destinationRectangle.Y <= 0 - _height)
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
            SoundEffectInstance instance = _soundEffect.CreateInstance();

            instance.Volume = 0.01f;
            instance.Play();
        }
    }
}
