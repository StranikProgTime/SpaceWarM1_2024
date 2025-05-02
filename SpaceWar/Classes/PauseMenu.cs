using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class PauseMenu : Menu
    {
        public event Action OnPlayingResume;
        public event Action OnSaveGame;

        public PauseMenu(int widthScreen, int heightScreen) : base(widthScreen, heightScreen)
        {
            _buttonList.Add(new Label(Vector2.Zero, "Resume", Color.White));
            _buttonList.Add(new Label(Vector2.Zero, "Save Game", Color.White));
            _buttonList.Add(new Label(Vector2.Zero, "Exit to menu", Color.White));
        }

        protected override void PressedEnter()
        {
            if (_selected == 0) // Resume
            {
                if (OnPlayingResume != null)
                {
                    OnPlayingResume();
                }
            }
            else if (_selected == 1)
            {
                if (OnSaveGame != null)
                {
                    OnSaveGame();
                }
            }
            else if (_selected == 2) // Exit to menu
            {
                Game1.gameMode = GameMode.Menu;
            }
        }
    }
}
