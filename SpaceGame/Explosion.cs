using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public class Explosion : GameObject
    {
        public override List<Collider2D> Colliders => new();   

        public Explosion(Vector2 position,float scale)
        {
            this.position = position;
            this.scale = scale;
        }
        public override void LoadContent(ContentManager content)
        {
            SoundEffect explosionSound = content.Load<SoundEffect>("Explosion");
            SoundEffectInstance explosionSound2 = explosionSound.CreateInstance();
            explosionSound2.Pitch = (float)new Random().NextDouble();
            explosionSound2.Play();
            sprites = new Texture2D[16];
            for (int i = 0; i < 16; i++)
            {
                sprites[i] = content.Load<Texture2D>($"explosion00{i + 1}");
            }
            fps = 30;
            sprite = sprites[0];
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);

        }
        public override void Update(GameTime gameTime)
        {
            if (Animate(gameTime,false))
            {
                GameWorld.Despawn(this);
            }
        }
    }
}
