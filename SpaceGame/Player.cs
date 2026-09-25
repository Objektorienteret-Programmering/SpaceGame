using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public class Player : GameObject
    {
        private float speed = 200f; // Speed of the player
        private Vector2 velocity = Vector2.Zero; // Velocity of the player

        private int screenWidth;
        private int screenHeight;

        public Player(int scrrenWidth, int screenHeight)
        {
            this.screenWidth = scrrenWidth;
            this.screenHeight = screenHeight;
          
        }

        override public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            base.LoadContent(content);
            position = new Vector2(screenWidth / 2f, screenHeight - sprite.Height / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            // Update logic for the player
            HandleInput();
            Move(gameTime);
            HandleScreenBounds();
        }

        private void HandleInput()
        {
            velocity = Vector2.Zero;

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.W))
            {
                velocity.Y = -1; // Move up
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y = 1; // Move down
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -1; // Move left
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = 1; // Move right
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize(); // Normalize the velocity to maintain consistent speed
            }
        }
        private void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position+=(velocity*speed) * deltaTime;
        }
        private void HandleScreenBounds()
        {
            float halfWidth = sprite.Width / 2f;
            float halfHeight = sprite.Height / 2f;

            position.X = MathHelper.Clamp(position.X, halfWidth, screenWidth - halfWidth);
            position.Y = MathHelper.Clamp(position.Y,halfHeight, screenHeight - halfHeight);
        }
    }
}
