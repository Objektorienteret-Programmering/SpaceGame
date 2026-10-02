using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    /// <summary>
    /// 
    /// </summary>
    public class Player : GameObject
    {

        private float speed = 200f; // Speed of the player
        private Vector2 velocity = Vector2.Zero; // Velocity of the player
        public int Health { get; private set; } = 3;
        private int screenWidth;
        private int screenHeight;
        private Texture2D[] leftSprites;
        private Texture2D[] rightSprites;
        private Texture2D[] forwardSprites;

        private float shootCooldown = 0.2f;
        private float shootTimer = 0f;

        private SoundEffect laserSound;
        public override List<Collider2D> Colliders
        {
            get
            {
                return new List<Collider2D> {
            //Player collider
            new BoxCollider { Bounds = new Rectangle(
            (int)(position.X - origin.X),
            (int)(position.Y - origin.Y),
            sprite.Width,
            sprite.Height-70) },
            //Flame collider
            new BoxCollider { Bounds = new Rectangle(
                (int)(position.X - origin.X+35),
                (int)(position.Y - origin.Y+80),
                sprite.Width-70,
                sprite.Height-120) } };
            }
        }

        public Player(int scrrenWidth, int screenHeight)
        {
            this.screenWidth = scrrenWidth;
            this.screenHeight = screenHeight;
            leftSprites = new Texture2D[4];
            rightSprites = new Texture2D[4];

            fps = 10;
        }

        override public void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            laserSound = content.Load<SoundEffect>("sfx_laser1");
            position = new Vector2(screenWidth / 2f, screenHeight - sprite.Height / 2f);

            sprites = new Texture2D[4];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = content.Load<Texture2D>($"{i + 1}fwd");
                leftSprites[i] = content.Load<Texture2D>($"{i + 1}lft");
                rightSprites[i] = content.Load<Texture2D>($"{i + 1}rght");
            }
            forwardSprites = sprites;
        }

        public override void Update(GameTime gameTime)
        {
            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            HandleInput();
            Move(gameTime);
            HandleScreenBounds();
            Animate(gameTime);
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
                sprites = leftSprites; // Set to left-facing sprites
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = 1; // Move right
                sprites = rightSprites; // Set to right-facing sprites
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && shootTimer <= 0f)
            {
                FireLaser();
                shootTimer = shootCooldown;
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize(); // Normalize the velocity to maintain consistent speed
            }
            else
            {
                sprites = forwardSprites;
            }
        }

        private void FireLaser()
        {
            Vector2 laserPosition = new Vector2(position.X, position.Y - sprite.Height / 2f);
            Laser laser = new Laser(laserPosition);
            GameWorld.Spawn(laser);
            laserSound.Play();
        }
        private void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += (velocity * speed) * deltaTime;
        }
        private void HandleScreenBounds()
        {
            float halfWidth = sprite.Width / 2f;
            float halfHeight = sprite.Height / 2f;

            position.X = MathHelper.Clamp(position.X, halfWidth, screenWidth - halfWidth);
            position.Y = MathHelper.Clamp(position.Y, halfHeight, screenHeight - halfHeight);
        }

        public override void OnCollisionEnter(GameObject other)
        {
            base.OnCollisionEnter(other);
            if (other is Enemy)
            {
                Health--;
                if (Health<=0)
                {
                    GameWorld.Despawn(this);
                }
            }
        }
    }
}
