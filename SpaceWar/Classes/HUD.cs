using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar.Classes
{
    public class HUD
    {
        private HealthBar _healthBar;
        private Label _labelScore;

        public HUD()
        {
            Vector2 position = new Vector2(20, 20);

            _healthBar = new HealthBar(position, 10, 150, 15);

            _labelScore = new Label(
                new Vector2(
                    position.X,
                    position.Y + _healthBar.DestinationRectanle.Height + 20
                ),
                "Score: 0",
                Color.White
            );
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            _healthBar.LoadContent(graphics);
            _labelScore.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _healthBar.Draw(spriteBatch);
            _labelScore.Draw(spriteBatch);
        }

        public void OnPlayerTakeDamage()
        {
            _healthBar.NumParts--;
        }
    }
}
