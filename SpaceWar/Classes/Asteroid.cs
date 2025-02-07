using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class Asteroid
    {
        private Texture2D _texture;
        private Vector2 _position;

        public Asteroid() : this(Vector2.Zero)
        {
            
        }

        public Asteroid(Vector2 position)
        {
            _texture = null;
            _position = position;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("asteroid");
        }

        public void Update()
        {
            _position.Y += 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
