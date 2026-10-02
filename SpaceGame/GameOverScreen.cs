using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public class GameOverScreen
    {
        private Texture2D background;
        private SpriteFont font;

        private int score;
        private int screenWidth;
        private int screenHeight;

        public GameOverScreen(int score, int screenWidth, int screenHeight)
        {
            this.score = score;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }
        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("Overlay");
            font = content.Load<SpriteFont>("font");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 center = new Vector2(
                screenWidth / 2f,
                screenHeight / 2f);

            // Centrer overlay
            Vector2 backgroundPosition = center - new Vector2(
                background.Width / 2f,
                background.Height / 2f);

            spriteBatch.Draw(background, backgroundPosition, Color.White);

            DrawCenteredText(spriteBatch,"GAME OVER", center.Y - 60);

            DrawCenteredText( spriteBatch, $"Score: {score}", center.Y);

            DrawCenteredText( spriteBatch,"Press ENTER to restart", center.Y + 60);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, float y)
        {
            Vector2 textSize = font.MeasureString(text);

            Vector2 position = new Vector2((screenWidth - textSize.X) / 2f, y);

            spriteBatch.DrawString(font,text, position, Color.White);
        }
    }
}
