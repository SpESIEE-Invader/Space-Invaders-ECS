using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Hitbox component can be added to an entity 
    /// an entity implementing this component gonna posses an hitbox,
    /// an hitbox is simply a rectangle, when two rectangle (hitbox) intersect 
    /// there is gonna be a in depth check, pixel by pixel to determine collision
    /// </summary>
    class HitboxComponent
    {
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get => this.hitbox;
            set => this.hitbox = value;
        }

        public HitboxComponent(Vec2 position, int width, int height)
        {
            this.hitbox = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), width, height);
        }
    }
}
