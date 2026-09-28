using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame
{
    public class Enemy : GameObject
    {
        private string spriteName;
        private float speed;

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

        public virtual float GetTop()
        {
            return position.Y - origin.Y;
        }
    }
}
