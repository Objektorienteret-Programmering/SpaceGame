using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceGame
{
    internal class HUD : GameObject
    {
        private Player player;
        private SpriteFont font;
        public override List<Collider2D> Colliders => new();
        public HUD(Player player)
        {
            this.player = player;
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Font");
        }
        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                        font,
                        $"Lives: {player.Health}",
                        new Vector2(20, 20),
                        Color.White);

            spriteBatch.DrawString(
                        font,
                        $"Score: {GameWorld.Score}",
                        new Vector2(20, 50),
                        Color.White);
        }
    }
}
