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
    public abstract class GameObject
    {
        protected Texture2D sprite;
        protected Vector2 position = Vector2.Zero;
        protected Vector2 origin;

        protected Texture2D[] sprites;
        protected int fps;
        private float timeElapsed;
        private int currentIndex;
        public virtual void LoadContent(ContentManager content)
        {
            this.sprite = content.Load<Texture2D>("1fwd");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);          
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public abstract void Update(GameTime gameTime);

        protected void Animate(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed >= 1f / fps)
            {
                currentIndex = (currentIndex + 1) % sprites.Length;
                sprite = sprites[currentIndex];
                timeElapsed = 0f;
            }
        }
    }
}
