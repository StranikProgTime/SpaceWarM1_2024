using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceWar.Classes
{
    public class MainMenu
    {
        private List<Label> _buttonList = new List<Label>();
        private int _selected;

        public MainMenu()
        {
            _selected = 0;

            _buttonList.Add(new Label(new Vector2(0, 0), "Play", Color.Yellow));
            _buttonList.Add(new Label(new Vector2(0, 40), "Exit", Color.White));
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Label button in _buttonList)
            {
                button.LoadContent(content);
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Label button in _buttonList)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
