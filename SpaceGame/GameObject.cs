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
    public class GameObject
    {
        private Texture2D sprite;
        private Vector2 position = Vector2.Zero;

        public void LoadContent(ContentManager content)
        {
            this.sprite = content.Load<Texture2D>("1fwd");  
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
