using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Position component can be added to an entity 
    /// an entity implementing this component has a position on the screen, that can be fixed or not.
    /// </summary>
    class PositionComponent
    {
        private Vec2 position;
        public Vec2 Position
        {
            get => new Vec2(position.X, position.Y);
            set => this.position = value; 
        }

        public PositionComponent(Vec2 position)
        {
            this.position = position;
        }
    }
}
