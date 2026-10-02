using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceGame
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        private static List<GameObject> removedGameObjects = new List<GameObject>();
        private Random random = new Random();
        private float enemySpawnTimer = 5f; // Spawn immediately on the first update.
        private string[] enemySprites = { "enemyBlack1", "enemyBlue2", "enemyGreen3", "enemyRed4", "meteorBrown_big1" };
        Texture2D pixel;
        Texture2D circleSprite;
        Player player;
        public enum GameState
        {
            Playing,
            GameOver
        }
        private GameOverScreen gameOverScreen;
        private static GameState gameState = GameState.Playing;
        private static int score;
        public static int Score
        {
            get { return score; }
        }

        public static void AddScore(int points)
        {
            score += points;
        }
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
            circleSprite = Content.Load<Texture2D>("CircleTexture");
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            Restart();       
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (gameState == GameState.Playing)
            {
                enemySpawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (enemySpawnTimer >= 2f)
                {
                    enemySpawnTimer = 0f;
                    SpawnEnemy();
                }

                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(gameTime);
                }
                CheckCollisions();

                foreach (GameObject gameObject in removedGameObjects)
                {
                    gameObject.ClearCollisions();
                    gameObjects.Remove(gameObject);
                }
                removedGameObjects.Clear();

                foreach (GameObject gameObject in newGameObjects)
                {
                    gameObject.LoadContent(Content);
                    gameObjects.Add(gameObject);
                }
                newGameObjects.Clear();
            }
            else if (gameState == GameState.GameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Restart();
                }
            }
   
            base.Update(gameTime);
        }

        private void SpawnEnemy()
        {
            string spriteName = enemySprites[random.Next(0, 5)];
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
            newGameObjects.Add(enemy);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (gameState == GameState.GameOver)
            {
                if (gameOverScreen == null)
                {
                    gameOverScreen = new GameOverScreen(Score, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    gameOverScreen.LoadContent(Content);
                }
                gameOverScreen.Draw(_spriteBatch);
            }
            else if (gameState == GameState.Playing)
            {
                foreach (var gameObject in gameObjects)
                {
                    // Draw each game object here
                    gameObject.Draw(_spriteBatch);
#if DEBUG
                    DrawColliders(gameObject);
#endif
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
        private void Restart()
        {
            removedGameObjects.Clear();
            gameObjects.Clear();
            newGameObjects.Clear();

            score = 0;
            enemySpawnTimer = 0f;     

            player = new Player(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Spawn(player);
            HUD hud = new HUD(player);
            Spawn(hud);
            gameOverScreen = null;
            gameState = GameState.Playing;
        }
        public static void SetGameOver()
        {
            gameState = GameState.GameOver;
        }
        private void DrawColliders(GameObject gameObject)
        {
            foreach (Collider2D collider in gameObject.Colliders)
            {
                if (collider is BoxCollider boxCollider)
                {
                    var box = boxCollider.Bounds;
                    _spriteBatch.Draw(pixel,
                        new Rectangle(box.Left, box.Top, box.Width, 1),
                        Color.Red);

                    _spriteBatch.Draw(pixel,
                        new Rectangle(box.Left, box.Bottom, box.Width, 1),
                        Color.Red);

                    _spriteBatch.Draw(pixel,
                        new Rectangle(box.Left, box.Top, 1, box.Height),
                        Color.Red);

                    _spriteBatch.Draw(pixel,
                        new Rectangle(box.Right, box.Top, 1, box.Height),
                        Color.Red);
                }
                else if (collider is CircleCollider circleCollider)
                {
                    float diameter = circleCollider.Radius * 2;  // 120
                    float scale = diameter / circleSprite.Width; // 120 / 100 = 1.2
                    _spriteBatch.Draw(circleSprite,
                circleCollider.Center,
                null,
                Color.Red,
                0f,
                new Vector2(circleSprite.Width / 2f, circleSprite.Height / 2f),
                scale,
                SpriteEffects.None,
                0f);
                }

            }
        }
        public static void Spawn(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        public static void Despawn(GameObject gameObject)
        {
            removedGameObjects.Add(gameObject);
        }
        private void CheckCollisions()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    gameObjects[i].CheckCollision(gameObjects[j]);
                }
            }
        }
    }
}




