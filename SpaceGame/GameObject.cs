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
    /// <summary>
    /// Represents an object in the game.
    /// </summary>
    public abstract class GameObject
    {
        protected Texture2D sprite;
        protected Vector2 position = Vector2.Zero;
        protected Vector2 origin;
        protected float rotation;
        protected float scale = 1f;
        protected Texture2D[] sprites;
        protected int fps;
        private float timeElapsed;
        private int currentIndex;
        private HashSet<GameObject> currentCollisions = new();
        public virtual List<Collider2D> Colliders
        {
            get
            {
                return new List<Collider2D> { new BoxCollider { Bounds = new Rectangle(
                (int)(position.X - origin.X),
                (int)(position.Y - origin.Y),
                sprite.Width,
                sprite.Height) }
                };
            }
        }
        public virtual void LoadContent(ContentManager content)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public abstract void Update(GameTime gameTime);

        protected bool Animate(GameTime gameTime, bool loop = true)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed >= 1f / fps)
            {
                currentIndex++;
                timeElapsed = 0f;

                if (currentIndex >= sprites.Length)
                {
                    if (loop)
                    {
                        currentIndex = 0;
                    }
                    else
                    {
                        currentIndex = sprites.Length - 1;
                        return true;
                    }
                }

                sprite = sprites[currentIndex];
            }

            return false;
        }
        private bool Intersects(GameObject other)
        {
            foreach (Collider2D collider in Colliders)
            {
                foreach (Collider2D otherCollider in other.Colliders)
                {
                    if (collider.Intersects(otherCollider))
                        return true;
                }
            }

            return false;
        }
        public void CheckCollision(GameObject other)
        {
            if (Intersects(other))
            {
                if (!currentCollisions.Contains(other))
                {
                    currentCollisions.Add(other);
                    other.currentCollisions.Add(this);

                    OnCollisionEnter(other);
                    other.OnCollisionEnter(this);
                }
                else
                {
                    OnCollisionStay(other);
                    other.OnCollisionStay(this);
                }
            }
            else if (currentCollisions.Contains(other))
            {
                currentCollisions.Remove(other);
                other.currentCollisions.Remove(this);

                OnCollisionExit(other);
                other.OnCollisionExit(this);
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
            Debug.WriteLine("current collisions from: " + this.GetType().Name + currentCollisions.Count);
        }

        public void ClearCollisions()
        {
            foreach (GameObject other in currentCollisions)
            {
                other.currentCollisions.Remove(this);
            }
            currentCollisions.Clear();
        }

    }
}
