using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceGame
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> removedGameObjects = new List<GameObject>();
        private Random random = new Random();
        private float enemySpawnTimer = 5f; // Spawn immediately on the first update.
        private string[] enemySprites = { "enemyBlack1", "enemyBlue2", "enemyGreen3", "enemyRed4", "meteorBrown_big1" };
        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameObject player = new Player(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            gameObjects.Add(player);

            foreach (var gameObject in gameObjects)
            {
                gameObject.LoadContent(Content);
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (enemySpawnTimer >= 5f)
            {
                enemySpawnTimer = 0f;
                SpawnEnemy();
            }

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);

                if (gameObject is Enemy)
                {
                    Enemy enemy = (Enemy)gameObject;
                    if (enemy.GetTop() > GraphicsDevice.Viewport.Height)
                    {
                        removedGameObjects.Add(enemy);
                    }
                }
            }

            // Vi ændrer først gameObjects, når gennemløbet er færdigt.
            foreach (GameObject gameObject in removedGameObjects)
            {
                gameObjects.Remove(gameObject);
            }
            removedGameObjects.Clear();

            foreach (GameObject gameObject in newGameObjects)
            {
                gameObjects.Add(gameObject);
            }
            newGameObjects.Clear();
            base.Update(gameTime);
        }

        private void SpawnEnemy()
        {
            string spriteName = enemySprites[random.Next(enemySprites.Length)];
            Texture2D sprite = Content.Load<Texture2D>(spriteName);
            float margin = sprite.Width / 2f;
            float y = -sprite.Height / 2f;

            if (spriteName == "meteorBrown_big1")
            {
                Vector2 halfSize = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
                margin = halfSize.Length();
                y = -margin;
            }

            int x = random.Next((int)margin + 1, GraphicsDevice.Viewport.Width - (int)margin);
            Vector2 spawnPosition = new Vector2(x, y);
            float speed = random.Next(75, 176);

            Enemy enemy;
            if (spriteName == "meteorBrown_big1")
            {
                enemy = new Asteroid(spawnPosition, speed);
            }
            else
            {
                enemy = new Enemy(spriteName, spawnPosition, speed);
            }
            enemy.LoadContent(Content);
            newGameObjects.Add(enemy);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            foreach (var gameObject in gameObjects)
            {
                // Draw each game object here
                gameObject.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
