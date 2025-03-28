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
        private int _widthScreen;
        private int _heightScreen;

        private KeyboardState _keyboardState;       // хранит нынешнее состояние клавиатуры
        private KeyboardState _prevKeyboardState;   // хранит предыдущее состояние клавиатуры

        public MainMenu(int widthScreen, int heightScreen)
        {
            _selected = 0;

            _widthScreen = widthScreen;
            _heightScreen = heightScreen;

            _buttonList.Add(new Label(new Vector2(0, 0), "Play", Color.White));
            _buttonList.Add(new Label(new Vector2(0, 40), "Exit", Color.White));
            _buttonList.Add(new Label(new Vector2(0, 40), "Hi. Im developer", Color.White));
        }

        public void LoadContent(ContentManager content)
        {
            int width = -100000;
            int height = 0;

            foreach (Label button in _buttonList)
            {
                button.LoadContent(content);

                if (button.SizeText.X > width)
                {
                    width = (int)button.SizeText.X;
                }

                height = height + (int)button.SizeText.Y;
            }

            height = height + 20 * (_buttonList.Count - 1);

            int x = _widthScreen / 2 - width / 2;
            int y = _heightScreen / 2 - height / 2;
            int offset = 0;

            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].Position = new Vector2(
                    x + (width - _buttonList[i].SizeText.X) / 2,
                    y + offset
                );

                offset += (int)_buttonList[i].SizeText.Y + 20;
                // offset = offset + (int)_buttonList[i].SizeText.Y + 20;
            }
        }

        public void Update()
        {
            _keyboardState = Keyboard.GetState();

            if (_prevKeyboardState.IsKeyUp(Keys.S) &&
                _keyboardState.IsKeyDown(Keys.S)
            )
            {
                _selected++;

                if (_selected >= _buttonList.Count)
                {
                    _selected = 0;
                }
            }

            if (_prevKeyboardState.IsKeyUp(Keys.W) &&
                _keyboardState.IsKeyDown(Keys.W)
            )
            {
                _selected--;

                if (_selected < 0)
                {
                    _selected = _buttonList.Count - 1;
                }
            }

            if (_keyboardState.IsKeyDown(Keys.Enter))
            {
                if (_selected == 0) // to Playing
                {
                    Game1.gameMode = GameMode.Playing;
                }
                else if (_selected == 1) // to Exit
                {
                    Game1.gameMode = GameMode.Exit;
                }
            }

            _prevKeyboardState = _keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _buttonList.Count; i++)
            {
                Color colorButton;

                if (i == _selected)
                {
                    colorButton = Color.Yellow;
                }
                else
                {
                    colorButton = Color.White;
                }

                _buttonList[i].Color = colorButton;
                _buttonList[i].Draw(spriteBatch);
            }
        }
    }
}
