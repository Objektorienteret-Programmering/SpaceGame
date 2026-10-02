using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceGame
{
    public class Asteroid : Enemy
    {
        private float rotationSpeed = 1f;
        public override List<Collider2D> Colliders
        {
            get
            {
                return new List<Collider2D> { new CircleCollider { 
                    Center = position, Radius = sprite.Width / 2f +10}
                };
            }
        }
        public Asteroid(Vector2 position, float speed)
            : base("meteorBrown_big1", position, speed)
        {
            pointOnDeath = 50;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rotation += rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
