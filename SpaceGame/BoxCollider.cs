using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
public class BoxCollider : Collider2D
{
    public Rectangle Bounds { get; set; }
    public override bool Intersects(Collider2D other)
    {
        if (other is BoxCollider box)
            return Intersects(this, box);

        if (other is CircleCollider circle)
            return Intersects(this, circle);

        return false;
    }
}
}
