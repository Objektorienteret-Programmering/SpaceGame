using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
public class CircleCollider : Collider2D
{
    public Vector2 Center { get; set; }
    public float Radius { get; set; }

    public override bool Intersects(Collider2D other)
    {
        if (other is CircleCollider circle)
            return Intersects(this, circle);

        if (other is BoxCollider box)
            return Intersects(box, this);

        return false;
    }
}
}
