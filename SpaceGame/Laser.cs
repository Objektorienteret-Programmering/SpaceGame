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
    public class Laser : GameObject
    {
        private float speed;
        private Vector2 velocity = new Vector2(0, -1); // Move upwards

        public override void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Laser");
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }
        public Laser(Vector2 position)
        {
            this.position = position;
            speed = 500f; // Set the speed of the laser
        }
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += (velocity * speed) * deltaTime;

            if (position.Y < 0)
            {
                GameWorld.Despawn(this);
            }
        }
    }
}
