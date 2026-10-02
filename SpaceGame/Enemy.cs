using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceGame
{
    public class Enemy : GameObject
    {
        private string spriteName;
        private float speed;
        protected int pointOnDeath = 100; 

        public Enemy(string spriteName, Vector2 position, float speed)
        {
            this.spriteName = spriteName;
            this.speed = speed;
            this.position = position;
        }

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteName);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
              
        public override void OnCollisionEnter(GameObject other)
        {
            base.OnCollisionEnter(other);
            if (other is Player)
            {
                GameWorld.Despawn(this);
                GameWorld.Spawn(new Explosion(position, 0.5f));
            }
            if (other is Laser)
            {
                GameWorld.AddScore(pointOnDeath);
                GameWorld.Spawn(new Explosion(position,0.5f));
                GameWorld.Despawn(this);
                GameWorld.Despawn(other);
            }
        }
    }
}
