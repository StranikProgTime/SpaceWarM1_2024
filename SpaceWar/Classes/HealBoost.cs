using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class HealBoost
    {
        private const int Speed = 2;
        private const int TimeRespawn = 100;

        private Texture2D _texture;
        private Vector2 _position;
        private bool _isMove;

        private int _timer = 0;

        public HealBoost()
        {
            
        }
    }
}
