using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected float rotation;
        protected Texture2D[] sprites;
        protected int fps;
        private float timeElapsed;
        private int currentIndex;
        private HashSet<GameObject> currentCollisions = new();
        public virtual Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)(position.X - origin.X),
                    (int)(position.Y - origin.Y),
                    sprite.Width,
                    sprite.Height);
            }
        }
        public virtual void LoadContent(ContentManager content)
        {
            this.sprite = content.Load<Texture2D>("1fwd");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);

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
      


        public void CheckCollision(GameObject other)
        {
            if (CollisionBox.Intersects(other.CollisionBox))
            {
                if (!currentCollisions.Contains(other))
                {
                    OnCollisionEnter(other);
                    currentCollisions.Add(other);
                }
                else
                {
                    OnCollisionStay(other);
                }
            }
            else
            {
                if (currentCollisions.Contains(other))
                {
                    OnCollisionExit(other);
                    currentCollisions.Remove(other);
                }
            }
        }

        public virtual void OnCollisionExit(GameObject other)
        {
            Debug.WriteLine($"Collision exited between {this.GetType().Name} and {other.GetType().Name}");
        }


        public virtual void OnCollisionStay(GameObject other)
        {
            //Debug.WriteLine($"Collision stayed between {this.GetType().Name} and {other.GetType().Name}");
        }

        public virtual void OnCollisionEnter(GameObject other)
        {
            Debug.WriteLine($"Collision entered between {this.GetType().Name} and {other.GetType().Name}");
        }

    }
}
