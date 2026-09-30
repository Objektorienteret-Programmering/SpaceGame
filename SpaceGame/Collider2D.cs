using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public abstract class Collider2D
    {
        public abstract bool Intersects(Collider2D other);
        protected bool Intersects(BoxCollider a, BoxCollider b)
        {
            return a.Bounds.Intersects(b.Bounds);
        }
        protected bool Intersects(CircleCollider a, CircleCollider b)
        {
            float distance = Vector2.Distance(a.Center, b.Center);

            return distance <= a.Radius + b.Radius;
        }
        protected bool Intersects(BoxCollider box, CircleCollider circle)
        {
            float closestX = MathHelper.Clamp(
                circle.Center.X,
                box.Bounds.Left,
                box.Bounds.Right);

            float closestY = MathHelper.Clamp(
                circle.Center.Y,
                box.Bounds.Top,
                box.Bounds.Bottom);

            Vector2 closestPoint = new Vector2(closestX, closestY);

            float distance = Vector2.Distance(
                circle.Center,
                closestPoint);

            return distance <= circle.Radius;
        }
    }
}
