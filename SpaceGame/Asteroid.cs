using Microsoft.Xna.Framework;

namespace SpaceGame
{
    public class Asteroid : Enemy
    {
        private float rotationSpeed = 1f;

        public Asteroid(Vector2 position, float speed)
            : base("meteorBrown_big1", position, speed)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rotation += rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override float GetTop()
        {
            // Afstanden fra midten til et hjørne giver plads til rotationen.
            return position.Y - origin.Length();
        }
    }
}
