using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class GameOver
    {
        private Label _label;
        private int _widthScreen;
        private int _heightScreen;

        public GameOver(int widthScreen, int heightScreen)
        {
            _label = new Label(
                new Vector2(250, 200),
                "GAME OVER :)", 
                Color.Red
            );

            _widthScreen = widthScreen;
            _heightScreen = heightScreen;
        }

        public void LoadContent(ContentManager content)
        {
            _label.LoadContent(content);

            _label.Position = new Vector2(
                _widthScreen / 2 - _label.SizeText.X / 2,
                _heightScreen / 2 - _label.SizeText.Y / 2
            );
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _label.Draw(spriteBatch);
        }
    }
}
