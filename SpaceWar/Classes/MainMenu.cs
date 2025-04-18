using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceWar.Classes
{
    public class MainMenu : Menu
    {
        public event Action OnPlayingStarted;

        public MainMenu(int widthScreen, int heightScreen) : base(widthScreen, heightScreen)
        {
            _buttonList.Add(new Label(Vector2.Zero, "Play", Color.White));
            _buttonList.Add(new Label(Vector2.Zero, "Load Game", Color.White));
            _buttonList.Add(new Label(Vector2.Zero, "Exit", Color.White));
        }

        protected override void PressedEnter()
        {
            if (_selected == 0) // to Playing
            {
                if (OnPlayingStarted != null)
                {
                    OnPlayingStarted();
                }
            }
            else if (_selected == 2) // to Exit
            {
                Game1.gameMode = GameMode.Exit;
            }
        }
    }
}
